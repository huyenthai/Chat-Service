namespace ChatService.Models
{
    public class SendMessageDto
    {
        public required string ReceiverId { get; set; }

        public string? Message { get; set; }

        public string? BlobName { get; set; }

        public string MessageType { get; set; } = "text";
    }
}
