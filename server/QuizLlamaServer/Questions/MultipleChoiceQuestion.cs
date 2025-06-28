namespace QuizLlamaServer.Questions;

public class MultipleChoiceQuestion : Question
{
    public List<MultipleChoiceAlternative> Alternatives { get; set; } = new();
    public List<int> CorrectAlternativeIndices { get; set; } = new();

    public override bool CheckAnswer(object answer)
    {
        if (answer is not int answerInt)
        {
            throw new ArgumentException("Answer must be an integer.", nameof(answer));
        }
        if (answerInt > Alternatives.Count || answerInt < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(answer), "Answer must be a valid index of an alternative.");
        }
        
        return CorrectAlternativeIndices.Contains(answerInt);
    }
}