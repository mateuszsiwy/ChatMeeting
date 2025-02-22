using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatMeeting.Core.Domain.Dtos
{
    public class MessageDTO
    {
        public Guid MessageId { get; set; } = Guid.NewGuid();
        public string Sender { get; set; }
        public string MessageText { get; set; }
        public Guid ChatId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now; 
    }
}
