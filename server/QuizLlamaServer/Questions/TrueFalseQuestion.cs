namespace QuizLlamaServer.Questions;

public class TrueFalseQuestion : Question
{
    public bool CorrectAnswer { get; set; }
    
    public TrueFalseQuestion()
    {
        QuestionType = QuestionType.TrueFalse;
    }
    
    public override bool CheckAnswer(object answer)
    {
        if (answer is not bool answerBool)
        {
            throw new ArgumentException("Answer must be a boolean.", nameof(answer));
        }
        
        return answerBool == CorrectAnswer;
    }
}
