using Confluent.Kafka;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatMeeting.Core.Domain.Interfaces.Producer
{
    public interface IKafkaProducer
    {
        Task Producer(string topic, Message<string, string> message);
    }
}
