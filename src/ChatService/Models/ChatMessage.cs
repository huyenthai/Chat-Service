using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ChatService.Models
{
    public class ChatMessage
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public required string Id { get; set; }

        [BsonRepresentation(BsonType.String)]
        public required string SenderId { get; set; }

        [BsonRepresentation(BsonType.String)]
        public required string ReceiverId { get; set; }

        public string? Message { get; set; }

        public string? BlobName { get; set; }

        public string MessageType { get; set; } = "text"; // "text" or "image"

        public DateTime TimeSent { get; set; }
    }
}
