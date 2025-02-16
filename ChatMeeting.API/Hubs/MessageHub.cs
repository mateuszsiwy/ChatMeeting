using ChatMeeting.Core.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace ChatMeeting.API.Hubs
{
    [Authorize]
    public class MessageHub : Hub
    {
        private readonly UserConnectionService _userConnectionService;
        private string _username => _userConnectionService.GetClaimValue(Context.User, ClaimTypes.NameIdentifier);

        public MessageHub(UserConnectionService userConnectionService)
        {
            _userConnectionService = userConnectionService;
        }

        public override Task OnConnectedAsync()
        {
            _userConnectionService.AddConnection(_username, Context.ConnectionId);
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            _userConnectionService.RemoveConnection(_username);

            return base.OnDisconnectedAsync(exception);
        }
    }
}
