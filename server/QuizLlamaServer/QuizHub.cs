using Microsoft.AspNetCore.SignalR;

namespace QuizLlamaServer;

public class QuizHub : Hub
{
    private readonly GameService _gameService;

    private ILogger<QuizHub> _logger;

    public QuizHub(ILogger<QuizHub> logger, GameService gameService)
    {
        _gameService = gameService;
        _logger = logger;
    }

    public async Task CreateGame()
    {
        _logger.LogInformation("CreateGame");
        var roomCode = _gameService.CreateGameGetRoomCode(Context.ConnectionId);
        if (string.IsNullOrWhiteSpace(roomCode))
        {
            _logger.LogError("FailedToCreateGame");
            await Clients.Caller.SendAsync("FailedToCreateGame");
            return;
        }

        Context.Items["RoomCode"] = roomCode;
        await Groups.AddToGroupAsync(Context.ConnectionId, roomCode);
        await Clients.Caller.SendAsync("GameCreated", roomCode);
    }

    public async Task StartGame()
    {
        _logger.LogInformation("StartGame");
        var roomCode = GetRoomCode();
        var game = _gameService.GetGame(roomCode);
        if (game is null || Context.ConnectionId != game.HostId)
            return;

        var a = Clients.Group(roomCode);
        await Clients.Group(roomCode).SendAsync("GameStarted", game.Questions[game.CurrentQuestionIndex]);
    }

    public async Task EndRound() // Should only be called from admin. And only as a possible override
    {
        _logger.LogInformation("Manual End Round");
        var game = _gameService.GetGame(GetRoomCode());
        if (game is null || Context.ConnectionId != game.HostId)
            return;
        await Clients.Group(GetRoomCode()).SendAsync("RoundEnded");
    }

    public async Task NextQuestion()
    {
        _logger.LogInformation("NextQuestion");
        var game = _gameService.GetGame(GetRoomCode());
        if (game is null || Context.ConnectionId != game.HostId)
            return;

        game.PlayersAnsweredCount = 0;
        if (game.CurrentQuestionIndex < game.Questions.Count - 1)
        {
            game.CurrentQuestionIndex++;
        }

        await Clients.OthersInGroup(GetRoomCode())
            .SendAsync("ReceiveQuestion", game.Questions[game.CurrentQuestionIndex]);
    }

    public async Task SubmitAnswer(string playerName, string answer)
    {
        _logger.LogInformation("Player {PlayerName} submitted answer: {answer}", playerName, answer);
        await Clients.Caller.SendAsync("AnswerReceived"); // Acknowledge
        var game = _gameService.GetGame(GetRoomCode());
        if (game is null)
            return;

        var newCount = game.IncrementPlayersAnsweredCount();
        await Clients.Group(GetRoomCode()).SendAsync("UpdatePlayersAnsweredCounter", newCount);
    }

    public async Task BroadcastLeaderboard(object leaderboard)
    {
        _logger.LogInformation("Broadcasting leaderboard: {Leaderboard}", leaderboard);
        await Clients.All.SendAsync("ReceiveLeaderboard", leaderboard);
    }

    public async Task JoinGame(string roomCode, string nickname)
    {
        _logger.LogInformation("JoinGame");
        Context.Items["RoomCode"] = roomCode;
        var game = _gameService.GetGame(GetRoomCode());
        if (game is null)
        {
            await Clients.Caller.SendAsync("GameNotFound");
            return;
        }
        if (game.IsPlayerNameTaken(nickname))
        {
            await Clients.Caller.SendAsync("NicknameTaken");
            return;
        }
        
        var joinedGame = _gameService.JoinGame(roomCode, nickname, Context.ConnectionId);
        if (joinedGame is false)
        {
            await Clients.Caller.SendAsync("FailedToJoinGame");
        }

        await Groups.AddToGroupAsync(Context.ConnectionId, roomCode);
        await Clients.Caller.SendAsync("GameJoined");
        await Clients.Group(roomCode).SendAsync("PlayerJoined", Context.ConnectionId);
    }

    private string GetRoomCode()
    {
        if (Context.Items.TryGetValue("RoomCode", out var roomCode))
        {
            return roomCode as string ?? string.Empty;
        }

        throw new ArgumentException("No room code available");
    }
}