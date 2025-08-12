using Microsoft.AspNetCore.Mvc;
using QuizLlamaServer.Answers;
using QuizLlamaServer.Guesses;
using QuizLlamaServer.Questions;
using QuizLlamaServer.Solutions;

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
    
    [HttpGet("guess")]
    public ActionResult<Guess> GetGuess()
    {
        return Ok(new Guess());
    }
    
    [HttpGet("solution")]
    public ActionResult<Solution> GetSolution()
    {
        return Ok(new Solution());
    }
    
    [HttpGet("correctnesses")]
    public ActionResult<IEnumerable<Correctness>> GetCorrectnesses()
    {
        return Ok(Correctnesses);
    }
}