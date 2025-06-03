using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace ChatService.Hubs
{
    public class ChatHub : Hub
    {
        public override async Task OnConnectedAsync()
        {
            var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!string.IsNullOrEmpty(userId))
            {
                // Optional: log or track connection
                Console.WriteLine($"User {userId} connected to SignalR hub.");
            }

            await base.OnConnectedAsync();
        }
    }
}
