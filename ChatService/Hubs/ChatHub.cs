using Microsoft.AspNetCore.SignalR;

namespace ChatService.Hubs
{
    public class ChatHub: Hub
    {
        public override Task OnConnectedAsync()
        {
            return base.OnConnectedAsync();
        }
    }
}
