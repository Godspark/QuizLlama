using QuizLlamaServer.Answers;
using QuizLlamaServer.Guesses;

namespace QuizLlamaServer.Questions;

public class MultipleChoiceQuestion : Question
{
    public List<MultipleChoiceAlternative> Alternatives { get; init; } = new();
    public List<int> CorrectAlternativeIndices { get; init; } = new();

    public override Correctness CheckAnswer(Guess guess)
    {
        if (guess.MultipleChoiceIndex == null 
            || guess.MultipleChoiceIndex < 1 
            || guess.MultipleChoiceIndex > Alternatives.Count)
        {
            return Correctness.NotAnswered;
        }
        
        return CorrectAlternativeIndices.Contains(guess.MultipleChoiceIndex.Value) ? Correctness.Correct : Correctness.Incorrect;
    }
}