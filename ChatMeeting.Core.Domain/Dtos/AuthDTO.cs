using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatMeeting.Core.Domain.Dtos
{
    public class AuthDTO
    {
        public string Token { get; set; }
        public DateTime ExpiryDate { get; set; }
    }
}
