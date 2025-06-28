namespace QuizLlamaServer.Questions;

public class TypeAnswerQuestion : Question
{
    public List<string> CorrectAnswers { get; } = new();

    public bool MustBeExact { get; set; } = false;

    public override bool CheckAnswer(object answer)
    {
        if (answer is not string answerString)
        {
            throw new ArgumentException("Answer must be a string.", nameof(answer));
        }
        
        if (MustBeExact)
        {
            return CorrectAnswers.Contains(answerString);
        }
        
        return CorrectAnswers.Any(correctAnswer => 
            answerString.Equals(correctAnswer, StringComparison.OrdinalIgnoreCase));
    }
}
