
using ChatMeeting.Core.Domain;
using ChatMeeting.Core.Domain.Consts;
using ChatMeeting.Core.Domain.Dtos;
using ChatMeeting.Core.Domain.Models;
using ChatMeeting.Core.Domain.Options;
using Confluent.Kafka;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace ChatMeeting.MessageBroker
{
    public class KafkaConsumer : BackgroundService
    {
        private readonly KafkaOptions _kafkaOptions;
        private readonly ILogger<KafkaConsumer> _logger;
        private readonly IDbContextFactory<ChatDbContext> _dbContextFactory;

        public KafkaConsumer(IDbContextFactory<ChatDbContext> dbContextFactory, ILogger<KafkaConsumer> logger, IOptions<KafkaOptions> options)
        {
            _dbContextFactory = dbContextFactory;
            _logger = logger;
            _kafkaOptions = options.Value;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                await Consume(TopicKafka.Message, stoppingToken);
            } catch (Exception ex)
            {
                _logger.LogError(ex, $"An error ocurred while consuming messages");
            }
        }

        private async Task Consume(string message, CancellationToken stoppingToken)
        {
            var config = CreateConsumerConfig();
            using var consumer = new ConsumerBuilder<string, string>(config).Build();
            consumer.Subscribe(message);

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var consumeResult = consumer.Consume(stoppingToken);
                    await ProcessMessage(consumeResult.Message.Value);

                } catch(Exception ex)
                {
                    _logger.LogError(ex, $"An error ocurred while processing the message.");
                    await Task.Delay(1000, stoppingToken);
                }
            }
        }

        private async Task ProcessMessage(string value)
        {
            var messageDTO = JsonSerializer.Deserialize<MessageDTO>(value);
            var message = CreateMessage(messageDTO);
            await SaveMessageToDatabase(message);
        }

        private async Task SaveMessageToDatabase(Message message)
        {
            try
            {
                var dbContext = _dbContextFactory.CreateDbContext();
                await dbContext.Messages.AddAsync(message);
                await dbContext.SaveChangesAsync();
                _logger.LogInformation($"Message with Id {message.MessageId} saved");
            } catch(Exception ex)
            {
                _logger.LogError(ex, $"An erro ocurred while saving message to the database");
                throw;
            }
        }

        private Message CreateMessage(MessageDTO? messageDTO)
        {
            return new Message
            {
                MessageId = messageDTO.MessageId,
                SenderId = messageDTO.SenderId,
                CreatedAt = messageDTO.CreatedAt,
                MessageText = messageDTO.MessageText,
                ChatId = messageDTO.ChatId
            };
        }

        private ConsumerConfig CreateConsumerConfig()
        {
            return new ConsumerConfig
            {
                GroupId = GroupKafka.Message,
                BootstrapServers = _kafkaOptions.Url,
                AutoOffsetReset = AutoOffsetReset.Earliest
            };
        }
    }
}
