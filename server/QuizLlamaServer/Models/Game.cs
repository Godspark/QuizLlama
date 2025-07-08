using QuizLlamaServer.Questions;
using QuizLlamaServer.Users;

namespace QuizLlamaServer.Models;

public class Game(string hostId)
{
    public List<Player> Players { get; set; } = [];

    public int CurrentQuestionIndex { get; set; } = 0;
    private int _playersAnsweredCount = 0;

    public int PlayersAnsweredCount
    {
        get => _playersAnsweredCount;
        set => _playersAnsweredCount = value;
    }

    public List<Question> Questions { get; set; } = [];

    public string HostId { get; set; } = hostId;

    public bool AddPlayer(string nickname, string connectionId)
    {
        var player = new Player
        {
            Nickname = nickname,
            ConnectionId = connectionId
        };

        if (Players.Any(x => x.Nickname == nickname))
        {
            return false;
        }

        Players.Add(player);
        return true;
    }

    public bool IsPlayerNameTaken(string nickname)
    {
        return Players.Any(x => x.Nickname == nickname);
    }

    public int IncrementPlayersAnsweredCount()
    {
        return Interlocked.Increment(ref _playersAnsweredCount);
    }
}