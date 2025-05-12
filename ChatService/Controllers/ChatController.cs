using Microsoft.AspNetCore.Mvc;
using ChatService.Business.Interfaces;
using ChatService.Models;
using System.Globalization;
namespace ChatService.Controllers
{
    [ApiController]
    [Route("api/chat")]
    public class ChatController: ControllerBase
    {
        private readonly IChatService chatService;
        public ChatController(IChatService chatService)
        {
            this.chatService = chatService;
        }
        [HttpPost("send")]
        public async Task<IActionResult> SendMessages([FromBody] ChatMessage message)
        {
            if (message == null)
            {
                return BadRequest("Message cannot be null");
            }
            await chatService.SendMessageAsync(message);
            return Ok("Message sent successfully");
        }
        [HttpGet("history")]
        public async Task<IActionResult> GetChatHistory([FromQuery] string senderId, string recieverId)
        {
            var chatHistory = await chatService.GetChatHistoryAsync(senderId, recieverId);
            return Ok(chatHistory);
        }
    }
}
