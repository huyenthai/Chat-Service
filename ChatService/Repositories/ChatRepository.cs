using ChatService.Models;
using ChatService.Business.Interfaces;
using MongoDB.Driver;


namespace ChatService.Repositories
{
    public class ChatRepository : IChatRepository
    {
        private readonly IMongoCollection<ChatMessage> chatCollection;
        public ChatRepository(IConfiguration config)
        {
            var client = new MongoClient(config["MongoDb:ConnectionString"]);
            var database = client.GetDatabase(config["MongoDb:Database"]);
            chatCollection = database.GetCollection<ChatMessage>("messages");

        }
        //get chat history between two users, messages that sender and receiver exchanged
        public async Task<List<ChatMessage>> GetMessageAsync(string senderId, string receiverId)
        {
            return await chatCollection.Find(msg =>
            (msg.SenderId == senderId && msg.ReceiverId == receiverId) ||
            (msg.SenderId == receiverId && msg.ReceiverId == senderId))
            .SortBy(m => m.TimeSent)
            .ToListAsync();
        }

        public async Task SaveMessageSync(ChatMessage message)
        {
            await chatCollection.InsertOneAsync(message);
        }
    }
}
