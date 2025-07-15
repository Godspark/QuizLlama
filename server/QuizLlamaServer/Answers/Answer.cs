using QuizLlamaServer.Questions;
using QuizLlamaServer.Users;

namespace QuizLlamaServer.Answers;

public class Answer
{ 
    public Question Question { get; set; }
    public Player Player { get; set; }
    public object? AnswerValue { get; set; }
    public Correctness Correctness { get; set; }
}
