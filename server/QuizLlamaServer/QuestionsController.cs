using Microsoft.AspNetCore.Mvc;
using QuizLlamaServer.Questions;

namespace QuizLlamaServer;

[ApiController]
[Route("api/[controller]")]
public class QuestionsController : ControllerBase
{
    // Replace with your actual data source
    private static readonly List<Question> Questions = new()
    {
        new MultipleChoiceQuestion(),
        new TrueFalseQuestion(),
        new TypeAnswerQuestion()
    };

    [HttpGet]
    public ActionResult<IEnumerable<Question>> GetQuestions()
    {
        return Ok(Questions);
    }
}