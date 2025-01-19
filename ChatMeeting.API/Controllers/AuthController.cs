using ChatMeeting.Core.Domain.Dtos;
using ChatMeeting.Core.Domain.Interfaces.Repositories;
using ChatMeeting.Core.Domain.Interfaces.Services;
using ChatMeeting.Core.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ChatMeeting.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {

        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;
        public AuthController(IAuthService authService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        [HttpPut("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserDTO registerUser)
        {
            try
            {
                await _authService.RegisterUser(registerUser);
                return Ok(new { message = "User registered succesfully" });
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex.Message);
                return Conflict(new { message = ex.Message });
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, $"An error occured during registration of user: {registerUser.Username}");
                return StatusCode(500, "An unexpected error occured during registration");
            }
        }
    }
}
