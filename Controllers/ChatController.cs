using Microsoft.AspNetCore.Mvc;
using ChatService.Business.Interfaces;
using ChatService.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace ChatService.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/chat")]
    public class ChatController : ControllerBase
    {
        private readonly IChatService chatService;

        public ChatController(IChatService chatService)
        {
            this.chatService = chatService;
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendMessages([FromBody] SendMessageDto dto)
        {
            Console.WriteLine("Hit SendMessages endpoint");

            if (dto == null || string.IsNullOrWhiteSpace(dto.ReceiverId))
                return BadRequest("ReceiverId and message cannot be null.");

            var senderId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value?.Trim();
            if (string.IsNullOrWhiteSpace(senderId))
                return Unauthorized("Sender not authenticated.");

            var message = new ChatMessage
            {
                SenderId = senderId,
                ReceiverId = dto.ReceiverId.Trim(),
                Message = dto.Message,
                ImageUrl = dto.ImageUrl,
                MessageType = dto.MessageType,
                TimeSent = DateTime.UtcNow
            };

            await chatService.SendMessageAsync(message);
            return Ok("Message sent successfully");
        }



        [HttpGet("history")]
        public async Task<IActionResult> GetChatHistory([FromQuery] string receiverId)
        {
            var senderId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value?.Trim();
            receiverId = receiverId?.Trim();

            if (string.IsNullOrWhiteSpace(senderId) || string.IsNullOrWhiteSpace(receiverId))
                return BadRequest("Invalid sender or receiver");

            var chatHistory = await chatService.GetChatHistoryAsync(senderId, receiverId);
            return Ok(chatHistory);
        }

        [HttpGet("contacts")]
        public async Task<IActionResult> GetChatContacts()
        {
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value?.Trim();
            if (string.IsNullOrWhiteSpace(currentUserId))
                return Unauthorized("User not authenticated");

            var contactIds = await chatService.GetChatContactsAsync(currentUserId);
            return Ok(contactIds);
        }


    }
}
