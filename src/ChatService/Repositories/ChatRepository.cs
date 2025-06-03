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
            Console.WriteLine($"[ChatRepository] GetMessageAsync called");
            Console.WriteLine($"SenderId: '{senderId}' (Type: {senderId?.GetType()?.Name})");
            Console.WriteLine($"ReceiverId: '{receiverId}' (Type: {receiverId?.GetType()?.Name})");
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
        public async Task<List<string>> GetContactUserIdsAsync(string userId)
        {
            var sentContacts = await chatCollection
                .Find(msg => msg.SenderId == userId)
                .Project(msg => msg.ReceiverId)
                .ToListAsync();

            var receivedContacts = await chatCollection
                .Find(msg => msg.ReceiverId == userId)
                .Project(msg => msg.SenderId)
                .ToListAsync();

            var allContacts = sentContacts.Concat(receivedContacts).Distinct().ToList();
            return allContacts;
        }

    }
}
