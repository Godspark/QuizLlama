using QuizLlamaServer.Answers;
using QuizLlamaServer.Guesses;

namespace QuizLlamaServer.Questions;

public class TypeAnswerQuestion : Question
{
    public List<string> CorrectAnswers { get; } = new();

    public bool MustBeExact { get; set; } = false;

    public override Correctness CheckAnswer(Guess guess)
    {
        if (guess.TypeAnswerText == null)
        {
            return Correctness.NotAnswered;
        }

        if (MustBeExact)
        {
            return CorrectAnswers.Contains(guess.TypeAnswerText) ? Correctness.Correct : Correctness.Incorrect;
        }
        
        return CorrectAnswers.Any(correctAnswer => 
            guess.TypeAnswerText.Equals(correctAnswer, StringComparison.OrdinalIgnoreCase)) 
            ? Correctness.Correct 
            : Correctness.Incorrect;
    }
}
