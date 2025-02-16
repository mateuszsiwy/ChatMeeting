using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatMeeting.Core.Domain.Dtos
{
    public class ChatDTO
    {
        public Guid ChatId { get; set; }
        public string Name { get; set; }
        public HashSet<MessageDTO> ?Messages { get; set; }
    }
}
