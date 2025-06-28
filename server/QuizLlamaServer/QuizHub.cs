using System.Collections.Concurrent;
using Microsoft.AspNetCore.SignalR;

namespace QuizLlamaServer;

public class QuizHub : Hub
{
    private readonly ILogger<QuizHub> _logger;
    // private static readonly ConcurrentDictionary<string, bool> ConnectedClients = new();

    public QuizHub(ILogger<QuizHub> logger)
    {
        _logger = logger;
    }
    
    // public override Task OnConnectedAsync()
    // {
    //     ConnectedClients.TryAdd(Context.ConnectionId, true);
    //     return base.OnConnectedAsync();
    // }
    //
    // public override Task OnDisconnectedAsync(Exception? exception)
    // {
    //     ConnectedClients.TryRemove(Context.ConnectionId, out _);
    //     return base.OnDisconnectedAsync(exception);
    // }
    
    // Host sends a new question
    public async Task SendQuestion(string question, string[] options)
    {
        _logger.LogInformation("Sending question: {Question} with options: {Options}", question, string.Join(", ", options));
        // _logger.LogInformation("Connected clients: {Clients}", string.Join(", ", ConnectedClients.Keys));
        await Clients.All.SendAsync("ReceiveQuestion", question, options);
    }

    // Player submits an answer
    public async Task SubmitAnswer(string playerName, int selectedOption)
    {
        _logger.LogInformation("Player {PlayerName} submitted answer: {SelectedOption}", playerName, selectedOption);
        // You would process/store the answer on the server here
        await Clients.Caller.SendAsync("AnswerReceived"); // Acknowledge
        // Optionally: update host or all players
    }

    // Host can broadcast leaderboard updates
    public async Task BroadcastLeaderboard(object leaderboard)
    {
        _logger.LogInformation("Broadcasting leaderboard: {Leaderboard}", leaderboard);
        await Clients.All.SendAsync("ReceiveLeaderboard", leaderboard);
    }
}