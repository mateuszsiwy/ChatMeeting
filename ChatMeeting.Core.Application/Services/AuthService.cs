using ChatMeeting.Core.Domain.Dtos;
using ChatMeeting.Core.Domain.Interfaces.Repositories;
using ChatMeeting.Core.Domain.Interfaces.Services;
using ChatMeeting.Core.Domain.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ChatMeeting.Core.Application.Services
{
    public class AuthService: IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<AuthService> _logger;
        public AuthService(IUserRepository userRepository, ILogger<AuthService> logger)
        {
            _userRepository = userRepository;
            _logger = logger;
        }

        public async Task RegisterUser(RegisterUserDTO user)
        {
            try
            {
                var existingUser = await _userRepository.GetUserBylogin(user.Username);
                if (existingUser != null)
                {
                    _logger.LogWarning($"User with login {user.Username} already exists");
                    throw new InvalidOperationException("User with this login already exists");
                }
                var newUser = new User(user.Username, HashPassword(user.Password));

                await _userRepository.AddUser(newUser);
            } catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occured while registering user: {user.Username}");
                throw new InvalidProgramException();
            }
        }

        private string HashPassword(string password)
        {
            byte[] salt = new byte[128 / 8];
            using(var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            string hashed = Hash(password, salt);
            return $"{Convert.ToBase64String(salt)}:{hashed}";
        }

        private string Hash(string password, byte[] salt)
        {
            return Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256/8));
        }
    }
}
