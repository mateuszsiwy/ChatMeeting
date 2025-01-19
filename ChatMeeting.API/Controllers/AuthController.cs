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

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpGet]
        public async Task<JsonResult> GetUser()
        {
            await _authService.RegisterUser(new Core.Domain.Dtos.RegisterUserDTO()
            {
                Username = "testtest1234",
                Password = "password"
            });
            return Json("");
        }
    }
}
