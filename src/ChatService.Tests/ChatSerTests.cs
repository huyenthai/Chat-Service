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
        public async Task SendMessageAsync_ValidMessage_CallsSaveAndBroadcast()
        {
            // Arrange
            var message = new ChatMessage
            {
                SenderId = "user234",
                ReceiverId = "user123",
                Message = "Hi"
            };

            var fakeClientProxy = new FakeClientProxy();

            var mockClients = new Mock<IHubClients>();
            mockClients.Setup(c => c.User(message.ReceiverId)).Returns(fakeClientProxy);
            mockHub.Setup(h => h.Clients).Returns(mockClients.Object);

            var chatService = new ChatSer(mockRepo.Object, mockHub.Object);

            // Act
            await chatService.SendMessageAsync(message);

            // Assert
            mockRepo.Verify(r => r.SaveMessageSync(message), Times.Once);
            Assert.Equal("ReceiveMessage", fakeClientProxy.LastMethod);
            Assert.NotNull(fakeClientProxy.LastArgs);
            Assert.Single(fakeClientProxy.LastArgs);
            Assert.Equal(message, fakeClientProxy.LastArgs[0]);
        }



        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public async Task SendMessageAsync_InvalidMessage_ThrowsArgumentException(string receiverId)
        {
            var message = new ChatMessage
            {
                SenderId = "",
                ReceiverId = receiverId,
                Message = "test message"
            };

            await Assert.ThrowsAsync<ArgumentException>(() =>
                chatService.SendMessageAsync(message));
        }


        [Fact]
        public async Task SendMessageAsync_NullMessage_ThrowsArgumentException()
        {
            await Assert.ThrowsAsync<ArgumentException>(() => chatService.SendMessageAsync((ChatMessage)null!));
        }

        [Fact]
        public async Task GetChatHistoryAsync_ValidIds_ReturnsMessages()
        {
            var expected = new List<ChatMessage> { new ChatMessage { SenderId = "user234", ReceiverId = "user123", Message = "Hi" } };
            mockRepo.Setup(r => r.GetMessageAsync("user1", "user2")).ReturnsAsync(expected);

            var result = await chatService.GetChatHistoryAsync("user1", "user2");

            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(null, "user2")]
        [InlineData("user1", null)]
        [InlineData("", "user2")]
        [InlineData("user1", "")]
        public async Task GetChatHistoryAsync_InvalidInput_ThrowsArgumentException(string senderId, string receiverId)
        {
            await Assert.ThrowsAsync<ArgumentException>(() => chatService.GetChatHistoryAsync(senderId, receiverId));
        }

        [Fact]
        public async Task GetChatContactsAsync_ValidUserId_ReturnsContacts()
        {
            var expected = new List<string> { "user2", "user3" };
            mockRepo.Setup(r => r.GetContactUserIdsAsync("user1")).ReturnsAsync(expected);

            var result = await chatService.GetChatContactsAsync("user1");

            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public async Task GetChatContactsAsync_InvalidUserId_ThrowsArgumentException(string userId)
        {
            await Assert.ThrowsAsync<ArgumentException>(() => chatService.GetChatContactsAsync(userId));
        }
    }
}
