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

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginModel)
        {
            try
            {
                var authData = await _authService.GetToken(loginModel);
                return Ok(authData);
            } catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { message = ex.Message });
            } catch (Exception ex)
            {
                _logger.LogError($"An error occurred during login for user: {loginModel.Username}");
                return StatusCode(500, $"An unexpected error occurred during login: {ex.Message}");
            }
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
