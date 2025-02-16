using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatMeeting.Core.Domain.Exceptions
{
    public class ChatNotFoundException : Exception
    {
        public ChatNotFoundException(string chatName) : base($"Chat with name: {chatName} was not found")
        {

        }
    }
}
