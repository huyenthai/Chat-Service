namespace ChatService.Models
{
    public class SendMessageDto
    {
        public string ReceiverId { get; set; }
        public string? Message { get; set; }
        public string? ImageUrl { get; set; }
        public string MessageType { get; set; } = "text";

    }
}
