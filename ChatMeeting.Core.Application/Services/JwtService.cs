using ChatMeeting.Core.Domain.Dtos;
using ChatMeeting.Core.Domain.Interfaces.Services;
using ChatMeeting.Core.Domain.Models;
using ChatMeeting.Core.Domain.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ChatMeeting.Core.Application.Services
{
    public class JwtService : IJwtService
    {
        private readonly JwtSettingsOptions _jwtSettingsOptions;

        public JwtService(IOptions<JwtSettingsOptions> jwtSettingsOptions)
        {
            _jwtSettingsOptions = jwtSettingsOptions.Value;
            if (string.IsNullOrWhiteSpace(_jwtSettingsOptions.SecretKey))
            {
                throw new ArgumentException("JWT SecretKey is not configured.");
            }
            if (_jwtSettingsOptions.ExpiryInMinutes <= 0)
            {
                throw new ArgumentException("JWT ExpiryInMinutes must be greater than 0.");
            }

        }

        public AuthDTO GenerateJwtToken(User user)
        {
            var claims = GetClaims(user);
            var expiryDate = DateTime.Now.AddMinutes(_jwtSettingsOptions.ExpiryInMinutes);
            var creds = GetCredentials();
            var token = new JwtSecurityToken(
                claims: claims, // lista atrybutow ktore beda przechowywane w tokenie, ktore mozemy dekodowac
                expires: expiryDate, // data wygasniecia
                signingCredentials: creds // okresla informacje potrzebne do przypisania tokena jwt, zawiera algorytm jaki
                // zastosujemy do zaszyfrowania tokenu
                );

            return new AuthDTO
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                ExpiryDate = expiryDate
            };
        }

        private SigningCredentials GetCredentials()
        {
            var byteSecretKey = Encoding.ASCII.GetBytes(_jwtSettingsOptions.SecretKey );
            var key = new SymmetricSecurityKey(byteSecretKey);
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            return creds;
        }

        private Claim[] GetClaims(User user)
        {
            return new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Username),
                new Claim(JwtRegisteredClaimNames.Jti, user.Id.ToString())
            };
        }
    }
}
