using ChatMeeting.Core.Domain.Consts;
using ChatMeeting.Core.Domain.Interfaces.Producer;
using ChatMeeting.Core.Domain.Options;
using Confluent.Kafka;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatMeeting.Infrastructure.Producer
{
    public class KafkaProducer : IKafkaProducer
    {
        private readonly IProducer<string, string> _producer;
        private readonly ILogger<KafkaProducer> _logger;
        public KafkaProducer(IOptions<KafkaOptions> options, ILogger<KafkaProducer> logger)
        {
            var kafkaSetting = options.Value;

            var config = new ConsumerConfig
            {
                GroupId = GroupKafka.Message,
                BootstrapServers = kafkaSetting.Url,
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            _producer = new ProducerBuilder<string, string>(config).Build();
            _logger = logger;
        }

        public async Task Producer(string topic, Message<string, string> message)
        {
            try
            {
                await _producer.ProduceAsync(topic, message);
            }catch (Exception ex)
            {
                _logger.LogError($"System got an error during sending a message on topic: {topic}");
                throw;
            }
        }

    }
}
