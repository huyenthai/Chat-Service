using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace ChatService.Models
{
    public class ChatMessage
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string id { get; set; }
        [BsonRepresentation(BsonType.String)]
        public string SenderId { get; set; }
        [BsonRepresentation(BsonType.String)]
        public string ReceiverId { get; set; }
        public string Message { get; set; }
        public DateTime TimeSent { get; set; }
    }
}
