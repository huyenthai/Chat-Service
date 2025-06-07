using ChatService.Business.Interfaces;
using ChatService.Models;
using ChatService.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace ChatService.Business.Services
{
    public class ChatSer : IChatService
    {
        private readonly IChatRepository chatRepository;
        private readonly IHubContext<ChatHub> hub;

        public ChatSer(IChatRepository chatRepository, IHubContext<ChatHub> hub)
        {
            this.chatRepository = chatRepository;
            this.hub = hub;
        }

        public async Task SendMessageAsync(ChatMessage message)
        {
            if (message == null || string.IsNullOrWhiteSpace(message.ReceiverId))
                throw new ArgumentException("Invalid message or receiver ID");
            await chatRepository.SaveMessageSync(message);
            await hub.Clients
                .User(message.ReceiverId)
                .SendAsync("ReceiveMessage", message);
        }

        public async Task<List<ChatMessage>> GetChatHistoryAsync(string senderId, string receiverId)
        {
            if (string.IsNullOrWhiteSpace(senderId) || string.IsNullOrWhiteSpace(receiverId))
                throw new ArgumentException("Sender and Receiver IDs must be provided");

            return await chatRepository.GetMessageAsync(senderId, receiverId);
        }

        public async Task<List<string>> GetChatContactsAsync(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
                throw new ArgumentException("User ID is required");
            return await chatRepository.GetContactUserIdsAsync(userId);
        }
    }
}
