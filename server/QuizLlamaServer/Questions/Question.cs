using QuizLlamaServer.Answers;

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

    public virtual Correctness CheckAnswer(object answer)
    {
        return Correctness.NotAnswered;
    }
}