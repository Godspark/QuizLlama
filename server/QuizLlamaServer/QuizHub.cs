using Microsoft.AspNetCore.SignalR;
using QuizLlamaServer.Answers;

namespace QuizLlamaServer;

public class QuizHub : Hub
{
    private readonly GameService _gameService;

    private ILogger<QuizHub> _logger;
    
    private const string AdminGroupPostfix = "-admin";

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
        await Groups.AddToGroupAsync(Context.ConnectionId, roomCode+AdminGroupPostfix);
        await Groups.AddToGroupAsync(Context.ConnectionId, roomCode);
        await Clients.Caller.SendAsync("GameCreated", roomCode);
    }

    public async Task StartGame()
    {
        _logger.LogInformation("StartGame");
        var roomCode = GetRoomCode();
        var game = _gameService.GetGame(roomCode);
        if (game is null)
        {
            _logger.LogError("StartGame(): Game with roomcode: {RoomCode} is null", roomCode);
            return;
        }
        var adminConnectionId = game.Admin.ConnectionId;
        if (Context.ConnectionId != adminConnectionId)
        {
            _logger.LogError("StartGame(): Non-admin user: {ContextConnectionId} tried to start game in room {RoomCode}, where admin connectionId is {AdminConnectionId}", Context.ConnectionId, roomCode, adminConnectionId);
            return;
        }

        await Clients.Group(roomCode).SendAsync("GameStarted", game.CurrentQuestion);
    }

    public async Task EndRound()
    {
        var roomCode = GetRoomCode();
        var game = _gameService.GetGame(roomCode);
        if (game is null)
        {
            _logger.LogError("EndRound(): Game with roomcode: {RoomCode} is null", roomCode);
            return;
        }
        var adminConnectionId = game.Admin.ConnectionId;
        if (Context.ConnectionId != adminConnectionId)
        {
            _logger.LogError("Non-admin user: {ContextConnectionId} tried to end round in room {RoomCode}, where admin connectionId is {AdminConnectionId}", Context.ConnectionId, roomCode, adminConnectionId);
            return;
        }
        _logger.LogInformation("Manual End Round");
        
        var correctAnswers = game.GetCorrectAnswers();
        foreach (var player in game.Players)
        {
            await Clients.Client(player.ConnectionId).SendAsync("RoundEnded", correctAnswers, game.GetCorrectness(player), player.Score); //answerDistribution too
        }
        await Clients.Caller.SendAsync("RoundEnded", correctAnswers, game.GetAnswerDistribution(), game.GetScoreboard());
    }

    public async Task NextQuestion()
    {
        _logger.LogInformation("NextQuestion");
        var roomCode = GetRoomCode();
        var game = _gameService.GetGame(roomCode);
        if (game is null)
        {
            _logger.LogError("NextQuestion(): Game with roomcode: {RoomCode} is null", roomCode);
            return;
        }
        var adminConnectionId = game.Admin.ConnectionId;
        if (Context.ConnectionId != adminConnectionId)
        {
            _logger.LogError("NextQuestion(): Non-admin user: {ContextConnectionId} tried to advance question in room {RoomCode}, where admin connectionId is {AdminConnectionId}", Context.ConnectionId, roomCode, adminConnectionId);
            return;
        }
        
        if (!game.AdvanceQuestion())
        {
            _logger.LogInformation("NextQuestion(): Tried to advance question, but no more questions left.");
            return;
        }

        await Clients.Group(roomCode).SendAsync("ReceiveQuestion", game.CurrentQuestion);
    }

    public async Task SubmitAnswer(Guess guess)
    {
        _logger.LogInformation("Player {Context.ConnectionId} submitted answer: {guess}", Context.ConnectionId, guess);
        var roomCode = GetRoomCode();
        var game = _gameService.GetGame(roomCode);
        if (game is null)
        {
            _logger.LogError("SubmitAnswer(): Game with roomcode: {RoomCode} is null", roomCode);
            return;
        }

        var player = game.Players.FirstOrDefault(p => p.ConnectionId == Context.ConnectionId);
        if (player is null)
        {
            _logger.LogError("SubmitAnswer(): Player with connectionId: {ConnectionId} not found in game with roomcode: {RoomCode}", Context.ConnectionId, roomCode);
            return;
        }

        int newCount;
        try
        {
            newCount = game.PlayerAnswered(player, guess);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "SubmitAnswer(): Failed to answer question");
            return;
        }
        
        await Clients.Caller.SendAsync("AnswerReceived");
        await Clients.Group(roomCode).SendAsync("UpdatePlayersAnsweredCounter", newCount);
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