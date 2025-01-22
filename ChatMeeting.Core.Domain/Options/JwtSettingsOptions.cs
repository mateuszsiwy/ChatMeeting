using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatMeeting.Core.Domain.Options
{
    public class JwtSettingsOptions
    {
        public string SecretKey { get; set; } = string.Empty;
        public int ExpiryInMinutes { get; set; }
    }
}
