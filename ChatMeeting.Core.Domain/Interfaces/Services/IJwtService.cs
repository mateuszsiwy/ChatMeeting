using ChatMeeting.Core.Domain.Dtos;
using ChatMeeting.Core.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatMeeting.Core.Domain.Interfaces.Services
{
    public interface IJwtService
    {
        public AuthDTO GenerateJwtToken(User user);
    }
}
