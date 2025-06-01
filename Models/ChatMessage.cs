using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace ChatService.Models
{
    public class ChatMessage
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string SenderId { get; set; }
        public string ReceiverId { get; set; }
        public string? Message { get; set; }
        public string? ImageUrl { get; set; }  // For image support
        public string MessageType { get; set; } = "text"; // "text" or "image"
        public DateTime TimeSent { get; set; }
    }
}
