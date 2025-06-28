using System.Collections.Concurrent;
using Microsoft.AspNetCore.SignalR;
using QuizLlamaServer.Questions;

namespace QuizLlamaServer;

public class QuizHub: Hub
{
    public List<Question> Questions { get; set; } = new();
    private ILogger<QuizHub> _logger;

    public QuizHub(ILogger<QuizHub> logger)
    {
        _logger = logger;
        Questions.Add(new MultipleChoiceQuestion
        {
            QuestionId = Guid.NewGuid(),
            QuestionType = QuestionType.MultipleChoice,
            QuestionText = "What is the capital of France?",
            ImageUrl = "https://example.com/paris.jpg",
            Explanation = "Paris is the capital and most populous city of France.",
            CategoryId = 1,
            Difficulty = 1,
            Alternatives =
            [
                new MultipleChoiceAlternative { Text = "Berlin", Index = 0 },
                new MultipleChoiceAlternative { Text = "Madrid", Index = 1 },
                new MultipleChoiceAlternative { Text = "Paris", Index = 2 },
                new MultipleChoiceAlternative { Text = "Rome", Index = 3 }
            ],
        });
    }
    
    // Host sends a new question
    public async Task NextQuestion()
    {
        //only admin should be able to send questions
        _logger.LogInformation("NextQuestion");
        await Clients.All.SendAsync("ReceiveQuestion", Questions[0]);
    }

    // Player submits an answer
    public async Task SubmitAnswer(string playerName, string answer)
    {
        _logger.LogInformation("Player {PlayerName} submitted answer: {answer}", playerName, answer);
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