using QuizLlamaServer.Models;
using QuizLlamaServer.Questions;

namespace QuizLlamaServer;

public class GameService
{
    private readonly Dictionary<string, Game> _games = new();
    private static readonly Random Random = new();

    public string CreateGameGetRoomCode(string hostConnectionId)
    {
        var roomCode = CreateRoomCode();
        
        if (!_games.TryAdd(roomCode, new Game(hostConnectionId)
            {
                Questions =
                [
                    new MultipleChoiceQuestion
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
                        CorrectAlternativeIndices = [2],
                        MaxPoints = 1000
                    },
                    new TrueFalseQuestion
                    {
                        QuestionId = Guid.NewGuid(),
                        QuestionType = QuestionType.TrueFalse,
                        QuestionText = "The Earth is flat.",
                        ImageUrl = "https://example.com/earth.jpg",
                        Explanation = "The Earth is an oblate spheroid, not flat.",
                        CategoryId = 2,
                        Difficulty = 1,
                        CorrectAnswer = false,
                        MaxPoints = 1000
                    },
                    new TypeAnswerQuestion
                    {
                        QuestionId = Guid.NewGuid(),
                        QuestionType = QuestionType.TypeAnswer,
                        QuestionText = "What is the largest planet in our solar system?",
                        ImageUrl = "https://example.com/jupiter.jpg",
                        Explanation = "Jupiter is the largest planet in our solar system.",
                        CategoryId = 3,
                        Difficulty = 1,
                        CorrectAnswers = { "Jupiter" },
                        MaxPoints = 1000
                    }
                ]
            }))
        {
            throw new ArgumentException("Game with that code already exists");
        }

        return roomCode;
    }

    public Game? GetGame(string gameId)
    {
        return _games.GetValueOrDefault(gameId);
    }

    public bool JoinGame(string gameId, string nickname, string connectionId)
    {
        if (!_games.TryGetValue(gameId, out var game))
        {
            return false;
        }
        return game.AddPlayer(nickname, connectionId);
    }

    private static string CreateRoomCode()
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        var roomCode = new string(Enumerable.Repeat(chars, 6)
            .Select(s => s[Random.Next(s.Length)]).ToArray());

        return roomCode;
    }
}