namespace QuizLlamaServer.Answers;

public class Guess
{
    public int? MultipleChoiceIndex { get; set; }
    public bool? TrueFalse { get; set; }
    public string? TypeAnswerText { get; set; } = string.Empty;
}