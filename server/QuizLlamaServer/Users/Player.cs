namespace QuizLlamaServer.Users;

public class Player
{
    public Guid PlayerId { get; set; }
    public string Nickname { get; set; } = string.Empty;
    public string ConnectionId { get; set; } = string.Empty;
    public int Score { get; set; } = 0;
    public int CorrectAnswers { get; set; } = 0;
}