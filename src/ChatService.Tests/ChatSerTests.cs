using Xunit;
using Moq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using ChatService.Business.Services;
using ChatService.Business.Interfaces;
using ChatService.Models;
using ChatService.Hubs;
using System.Collections.Generic;

namespace ChatService.Tests
{
    public class ChatSerTests
    {
        private readonly Mock<IChatRepository> mockRepo;
        private readonly Mock<IHubContext<ChatHub>> mockHub;
        private readonly Mock<IClientProxy> mockClientProxy;
        private readonly ChatSer chatService;

        public ChatSerTests()
        {
            mockRepo = new Mock<IChatRepository>();
            mockHub = new Mock<IHubContext<ChatHub>>();
            mockClientProxy = new Mock<IClientProxy>();

            var mockClients = new Mock<IHubClients>();
            mockClients.Setup(c => c.User(It.IsAny<string>())).Returns(mockClientProxy.Object);
            mockHub.Setup(h => h.Clients).Returns(mockClients.Object);

            chatService = new ChatSer(mockRepo.Object, mockHub.Object);
        }

        [Fact]
        public async Task SendMessageAsync_CallsRepositoryAndHub()
        {
            var message = new ChatMessage { SenderId = "user1", ReceiverId = "user2", Message = "Hello" };

            await chatService.SendMessageAsync(message);

            mockRepo.Verify(r => r.SaveMessageSync(message), Times.Once);
            mockClientProxy.Verify(c => c.SendCoreAsync("ReceiveMessage",
                                                         It.Is<object[]>(o => o[0] == message),
                                                         default), Times.Once);
        }
        [Fact]
        public async Task GetChatHistoryAsync_ReturnsMessages()
        {
            var expectedMessages = new List<ChatMessage>
            {
                new ChatMessage
                {
                    Message = "Hi",
                    SenderId = "user1",
                    ReceiverId = "user2"
                }
            };

            mockRepo.Setup(r => r.GetMessageAsync("user1", "user2")).ReturnsAsync(expectedMessages);

            var result = await chatService.GetChatHistoryAsync("user1", "user2");

            Assert.Equal(expectedMessages, result);
        }



        [Fact]
        public async Task GetChatContactsAsync_ReturnsContactIds()
        {
            var expectedContacts = new List<string> { "user2", "user3" };
            mockRepo.Setup(r => r.GetContactUserIdsAsync("user1")).ReturnsAsync(expectedContacts);

            var result = await chatService.GetChatContactsAsync("user1");

            Assert.Equal(expectedContacts, result);
        }

    }
}
