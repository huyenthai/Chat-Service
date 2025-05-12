using ChatService.Models;


namespace ChatService.Business.Interfaces
{
    public interface IChatRepository
    {
        Task SaveMessageSync(ChatMessage message);
        Task<List<ChatMessage>> GetMessageAsync(string senderId, string receiverId);
    }
}
