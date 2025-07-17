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

    public static Answer Answer = new();

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
    
    [HttpGet("correctnesses")]
    public ActionResult<IEnumerable<Correctness>> GetCorrectnesses()
    {
        return Ok(Correctnesses);
    }
}