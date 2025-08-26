namespace QuizLlamaServer.Answers;

public class Solution
{
    public List<int>? MultipleChoiceSolutionIndices { get; set; }
    public bool? TrueFalseSolution { get; set; }
    public List<string>? TypeAnswerSolutions { get; set; } = [];
}