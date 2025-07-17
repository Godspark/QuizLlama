using QuizLlamaServer.Answers;

namespace QuizLlamaServer.Questions;

public class MultipleChoiceQuestion : Question
{
    public List<MultipleChoiceAlternative> Alternatives { get; set; } = new();
    public List<int> CorrectAlternativeIndices { get; set; } = new();

    public override Correctness CheckAnswer(object answer)
    {
        int intAnswer;
        try
        {
            intAnswer = (int)answer;
        }
        catch (Exception e)
        {
            throw new ArgumentException("Answer must be an integer.", nameof(answer));
        }
        if (intAnswer > Alternatives.Count || intAnswer < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(answer), "Answer must be a valid index of an alternative.");
        }
        
        return CorrectAlternativeIndices.Contains(intAnswer) ? Correctness.Correct : Correctness.Incorrect;
    }
}