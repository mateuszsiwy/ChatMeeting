using ChatMeeting.Core.Domain.Consts;
using ChatMeeting.Core.Domain.Options;
using Confluent.Kafka;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatMeeting.Infrastructure.Producer
{
    public class KafkaProducer
    {
        private readonly IProducer<string, string> _producer;
        public KafkaProducer(IOptions<KafkaOptions> options)
        {
            var kafkaSetting = options.Value;

            var config = new ConsumerConfig
            {
                GroupId = GroupKafka.Message,
                BootstrapServers = kafkaSetting.Url,
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            _producer = new ProducerBuilder<string, string>(config).Build();
        }

        public async Task Producer(string topic, Message<string, string> message)
        {
            try
            {
                await _producer.ProduceAsync(topic, message);
            }catch (Exception ex)
            {

            }
        }

    }
}
