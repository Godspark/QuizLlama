using Microsoft.AspNetCore.Mvc.ModelBinding;
using QuizLlamaServer.Questions;
using QuizLlamaServer.Users;

namespace QuizLlamaServer.Answers;

public abstract class Answer
{
    public Question Question { get; set; }
    
    [BindNever]
    public Player Player { get; set; }
    
    [BindNever]
    public Correctness Correctness { get; set; }
}
