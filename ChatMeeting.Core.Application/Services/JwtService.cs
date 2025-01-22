using ChatMeeting.Core.Domain.Dtos;
using ChatMeeting.Core.Domain.Models;
using ChatMeeting.Core.Domain.Options;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatMeeting.Core.Application.Services
{
    public class JwtService
    {
        private readonly JwtSettingsOptions _jwtSettingsOptions;

        public JwtService(IOptions<JwtSettingsOptions> jwtSettingsOptions)
        {
            _jwtSettingsOptions = jwtSettingsOptions.Value;
        }

        public AuthDTO GenerateJwtToken(User user)
        {
            var token = new JwtSecurityToken(
                claims: 
                )
        }
    }
}
