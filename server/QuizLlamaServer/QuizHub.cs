using Microsoft.AspNetCore.SignalR;
using QuizLlamaServer.Questions;
using QuizLlamaServer.Users;

namespace QuizLlamaServer;

public class QuizHub: Hub
{
    public List<Question> Questions { get; set; } = new();
    private static int _currentQuestionIndex = -1;
    private ILogger<QuizHub> _logger;
    public List<Player> Players { get; set; } = new();

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
        Questions.Add(new TrueFalseQuestion
        {
            QuestionId = Guid.NewGuid(),
            QuestionType = QuestionType.TrueFalse,
            QuestionText = "The Earth is flat.",
            ImageUrl = "https://example.com/earth.jpg",
            Explanation = "The Earth is an oblate spheroid, not flat.",
            CategoryId = 2,
            Difficulty = 1,
            CorrectAnswer = false
        });
    }
    
    public async Task NextQuestion()
    {
        _logger.LogInformation("NextQuestion");
        if (_currentQuestionIndex < Questions.Count - 1)
        {
            _currentQuestionIndex++;
        }
        await Clients.All.SendAsync("ReceiveQuestion", Questions[_currentQuestionIndex]);
    }
    
    public async Task EndRound() // Should only be called from admin. And only as a possible override
    {
        _logger.LogInformation("Manual End Round");
        await Clients.All.SendAsync("EndRound");
    }

    public async Task SubmitAnswer(string playerName, string answer)
    {
        _logger.LogInformation("Player {PlayerName} submitted answer: {answer}", playerName, answer);
        await Clients.Caller.SendAsync("AnswerReceived"); // Acknowledge
        await Clients.All.SendAsync("PlayerAnswered");
    }

    public async Task BroadcastLeaderboard(object leaderboard)
    {
        _logger.LogInformation("Broadcasting leaderboard: {Leaderboard}", leaderboard);
        await Clients.All.SendAsync("ReceiveLeaderboard", leaderboard);
    }
}