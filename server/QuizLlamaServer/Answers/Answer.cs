using QuizLlamaServer.Questions;
using QuizLlamaServer.Users;

namespace QuizLlamaServer.Answers;

public class Answer
{
    public required Question Question { get; init; }
    
    public required Player Player { get; init; }
    
    public Correctness Correctness { get; set; }
    
    public int PointsAwarded { get; set; }
    
    public int? MultipleChoiceIndex { get; set; }
    public bool? TrueFalse { get; set; }
    public string? TypeAnswerText { get; set; }
}
