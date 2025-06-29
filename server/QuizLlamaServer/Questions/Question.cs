using System.Runtime.Serialization;

namespace QuizLlamaServer.Questions;

public abstract class Question
{
    public Guid QuestionId { get; set; }
    public QuestionType QuestionType { get; set; }
    public string QuestionText { get; set; }
    public string ImageUrl { get; set; }
    public string Explanation { get; set; }
    
    public int CategoryId { get; set; }
    public int Difficulty { get; set; }

    public virtual bool CheckAnswer(object answer)
    {
        return false;
    }
}