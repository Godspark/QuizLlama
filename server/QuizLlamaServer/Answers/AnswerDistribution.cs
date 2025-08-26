namespace QuizLlamaServer.Answers;

public class AnswerDistribution
{
    public Dictionary<int, int> MultipleChoiceDistribution { get; set; } = new();
    public Dictionary<bool, int> TrueFalseDistribution { get; set; } = new();
    public Dictionary<string, int> TypeAnswerDistribution { get; set; } = new();
}