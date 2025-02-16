using ChatMeeting.Core.Domain.Exceptions;
using ChatMeeting.Core.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChatMeeting.API.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    [Authorize]
    public class ChatController : Controller
    {

        private readonly IChatService _chatService;
        private readonly ILogger<ChatController> _logger;

        public ChatController(IChatService chatService, ILogger<ChatController> logger)
        {
            _chatService = chatService;
            _logger = logger;
        }

        [HttpPost("GetPaginatedChat")]
        public async Task<IActionResult> GetPaginatedChat(string chatName, int pageNumber, int pageSize)
        {
            try
            {
                var chat = await _chatService.GetPaginatedChat(chatName, pageNumber, pageSize);

                return Json(chat);
            }catch (ChatNotFoundException ex)
            {
                return Conflict(ex.Message);
            }catch(Exception ex)
            {
                _logger.LogError(ex, $"Unexpected error occured");
                return BadRequest(ex);
            }
        }
    }
}
