namespace QuizLlamaServer.Questions;

public class MultipleChoiceAlternative
{
    public string Text { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    public int Index { get; set; } = -1;
}