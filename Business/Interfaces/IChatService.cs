using ChatService.Models;
namespace ChatService.Business.Interfaces
{
    public interface IChatService
    {

        Task SendMessageAsync(ChatMessage message);
        Task<List<ChatMessage>> GetChatHistoryAsync(string senderId, string receiverId);
        Task<List<string>> GetChatContactsAsync(string userId);

    }
}
