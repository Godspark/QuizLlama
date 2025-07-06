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
    private static int _playersAnsweredCounter = 0;

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

    public async Task StartGame()
    {
        _logger.LogInformation("StartGame");
        _currentQuestionIndex = 0;
        _playersAnsweredCounter = 0;
        await Clients.All.SendAsync("GameStarted", Questions[_currentQuestionIndex]);
    }
    
    public async Task EndRound() // Should only be called from admin. And only as a possible override
    {
        _logger.LogInformation("Manual End Round");
        await Clients.All.SendAsync("RoundEnded");
    }
    
    public async Task NextQuestion()
    {
        _logger.LogInformation("NextQuestion");
        _playersAnsweredCounter = 0;
        if (_currentQuestionIndex < Questions.Count - 1)
        {
            _currentQuestionIndex++;
        }
        await Clients.All.SendAsync("ReceiveQuestion", Questions[_currentQuestionIndex]);
    }

    public async Task SubmitAnswer(string playerName, string answer)
    {
        _logger.LogInformation("Player {PlayerName} submitted answer: {answer}", playerName, answer);
        _playersAnsweredCounter++;
        await Clients.Caller.SendAsync("AnswerReceived"); // Acknowledge
        await Clients.All.SendAsync("UpdatePlayersAnsweredCounter", _playersAnsweredCounter);
    }

    public async Task BroadcastLeaderboard(object leaderboard)
    {
        _logger.LogInformation("Broadcasting leaderboard: {Leaderboard}", leaderboard);
        await Clients.All.SendAsync("ReceiveLeaderboard", leaderboard);
    }
}