using ChatMeeting.Core.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ChatMeeting.API.Hubs
{
    [Authorize]
    public class MessageHub : Hub
    {
        private readonly UserConnectionService _userConnectionService;
        private string _username => _userConnectionService.GetClaimValue(Context.User, ClaimTypes.NameIdentifier);
        private string _userId => _userConnectionService.GetClaimValue(Context.User, JwtRegisteredClaimNames.Jti);
        private const string _mainChat = "Global";
        public MessageHub(UserConnectionService userConnectionService)
        {
            _userConnectionService = userConnectionService;
        }

        public override async Task OnConnectedAsync()
        {
            _userConnectionService.AddConnection(_username, Context.ConnectionId);
            await JoinChat(_mainChat);
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            _userConnectionService.RemoveConnection(_username);
            await LeaveChat(_mainChat);
            await base.OnDisconnectedAsync(exception);
        }

        public async Task SendMessageToChat(string chatId, string message)
        {
            await Clients.Group(_mainChat).SendAsync("ReceiveMessage", _username, message);
        }

        private async Task JoinChat(string chatName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, chatName);
        }


        private async Task LeaveChat(string chatName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, chatName);
        }
    }
}
