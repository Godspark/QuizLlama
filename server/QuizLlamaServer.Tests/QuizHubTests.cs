using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Moq;
using Xunit;

namespace QuizLlamaServer.Tests;

public class QuizHubTests
{
    [Fact]
    public async Task SendQuestion_ShouldBroadcastToAllClients()
    {
        // Arrange
        var mockClients = new Mock<IHubCallerClients>();
        var mockClientProxy = new Mock<IClientProxy>();
        mockClients.Setup(clients => clients.All).Returns(mockClientProxy.Object);

        var hub = new QuizHub
        {
            Clients = mockClients.Object
        };

        const string question = "What is 2+2?";
        var options = new[] { "3", "4", "5", "6" };

        // Act
        await hub.SendQuestion(question, options);

        // Assert
        mockClientProxy.Verify(
            x => x.SendCoreAsync(
                "ReceiveQuestion",
                It.Is<object[]>(o => o[0].Equals(question) && o[1].Equals(options)),
                default
            ),
            Times.Once
        );
    }
    //
    // [Fact]
    // public async Task SendQuestion_BroadcastsToClients()
    // {
    //     var builder = new WebHostBuilder().UseStartup<Program>();
    //     using var server = new TestServer(builder);
    //     var client = server.CreateClient();
    //
    //     var connection = new HubConnectionBuilder()
    //         .WithUrl("http://localhost/quizhub", o => o.HttpMessageHandlerFactory = _ => server.CreateHandler())
    //         .Build();
    //
    //     string receivedQuestion = null;
    //     connection.On<string, string[]>("ReceiveQuestion", (q, o) => receivedQuestion = q);
    //
    //     await connection.StartAsync();
    //     await connection.InvokeAsync("SendQuestion", "Test?", new[] { "A", "B" });
    //
    //     await Task.Delay(100); // Wait for message
    //
    //     Assert.Equal("Test?", receivedQuestion);
    //     await connection.StopAsync();
    // }
}