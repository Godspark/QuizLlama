using QuizLlamaServer.Answers;
using QuizLlamaServer.Guesses;

namespace QuizLlamaServer.Questions;

public class TrueFalseQuestion : Question
{
    public bool CorrectAnswer { get; set; }
    
    public TrueFalseQuestion()
    {
        QuestionType = QuestionType.TrueFalse;
    }
    
    public override Correctness CheckAnswer(Guess guess)
    {
        if (guess.TrueFalse == null)
        {
            return Correctness.NotAnswered;
        }
        
        return guess.TrueFalse == CorrectAnswer ? Correctness.Correct : Correctness.Incorrect;
    }
}
