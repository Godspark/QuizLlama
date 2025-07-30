using Microsoft.AspNetCore.Mvc;
using QuizLlamaServer.Answers;
using QuizLlamaServer.Questions;

namespace QuizLlamaServer;

[ApiController]
[Route("api/[controller]")]
public class QuestionsController : ControllerBase
{
    private static readonly List<Question> Questions = new()
    {
        new MultipleChoiceQuestion(),
        new TrueFalseQuestion(),
        new TypeAnswerQuestion()
    };
    
    private static readonly List<Answer> Answers = new()
    {
        new MultipleChoiceAnswer(),
        new TrueFalseAnswer(),
        new TypeAnswerAnswer()
    };

    private static readonly List<Correctness> Correctnesses = new()
    {
        Correctness.Correct,
        Correctness.Incorrect,
        Correctness.PartiallyCorrect,
        Correctness.NotAnswered
    };

    [HttpGet("questions")]
    public ActionResult<IEnumerable<Question>> GetQuestions()
    {
        return Ok(Questions);
    }
    
    [HttpGet("answers")]
    public ActionResult<IEnumerable<Answer>> GetAnswers()
    {
        return Ok(Answers);
    }
    
    [HttpGet("correctnesses")]
    public ActionResult<IEnumerable<Correctness>> GetCorrectnesses()
    {
        return Ok(Correctnesses);
    }
}