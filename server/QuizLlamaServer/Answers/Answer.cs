using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using QuizLlamaServer.Questions;
using QuizLlamaServer.Users;

namespace QuizLlamaServer.Answers;

[JsonPolymorphic]
[JsonDerivedType(typeof(MultipleChoiceAnswer), nameof(MultipleChoiceAnswer))]
[JsonDerivedType(typeof(TypeAnswerAnswer), nameof(TypeAnswerAnswer))]
[JsonDerivedType(typeof(TrueFalseAnswer), nameof(TrueFalseAnswer))]
public abstract class Answer
{
    public Question Question { get; set; }
    
    [BindNever]
    public Player Player { get; set; }
    
    [BindNever]
    public Correctness Correctness { get; set; }
}
