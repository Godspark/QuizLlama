using QuizLlamaServer.Answers;
using QuizLlamaServer.Questions;
using QuizLlamaServer.Users;

namespace QuizLlamaServer.Models;

public class Game
{
    public Game(string adminConnectionId)
    {
        Admin = new Admin { ConnectionId = adminConnectionId };
    }
    public Admin Admin { get; set; }
    public List<Player> Players { get; set; } = [];

    private int _currentQuestionIndex, _playersAnsweredCount = 0;

    public int PlayersAnsweredCount
    {
        get => _playersAnsweredCount;
        set => _playersAnsweredCount = value;
    }

    public List<Question> Questions { get; set; } = [];
    
    private List<Answer> Answers { get; set; } = [];
    
    public Question CurrentQuestion => Questions[_currentQuestionIndex];

    public bool AdvanceQuestion()
    {
        PlayersAnsweredCount = 0;
        if (_currentQuestionIndex < Questions.Count - 1)
        {
            _currentQuestionIndex++;
            return true;
        }
        return false;
    }
    
    public bool AddPlayer(string nickname, string connectionId)
    {
        var player = new Player
        {
            Nickname = nickname,
            ConnectionId = connectionId
        };

        if (Players.Any(x => x.Nickname == nickname))
        {
            return false;
        }

        Players.Add(player);
        return true;
    }

    public bool IsPlayerNameTaken(string nickname)
    {
        return Players.Any(x => x.Nickname == nickname);
    }
    
    public int PlayerAnswered(Player player, object answer)
    {
        Answers.Add(new Answer
        {
            Player = player,
            Question = CurrentQuestion,
            AnswerValue = answer,
            Correctness = CurrentQuestion.CheckAnswer(answer)
        });
        return Interlocked.Increment(ref _playersAnsweredCount);
    }

    public Correctness GetCorrectness(Player player)
    {
        return Answers.First(x => x.Player == player && x.Question == CurrentQuestion).Correctness;
    }

    public object GetCorrectAnswers()
    {
        return CurrentQuestion.QuestionType switch
        {
            QuestionType.TrueFalse => ((TrueFalseQuestion)CurrentQuestion).CorrectAnswer,
            QuestionType.TypeAnswer => ((TypeAnswerQuestion)CurrentQuestion).CorrectAnswers,
            QuestionType.MultipleChoice => ((MultipleChoiceQuestion)CurrentQuestion).CorrectAlternativeIndices,
            _ => throw new InvalidOperationException("Unsupported question type.")
        };
    }
    
    // public object GetAnswerDistribution()
    // {
    //     return CurrentQuestion.QuestionType switch
    //     {
    //         QuestionType.TrueFalse => ((TrueFalseQuestion)CurrentQuestion).CorrectAnswer,
    //         QuestionType.TypeAnswer => ((TypeAnswerQuestion)CurrentQuestion).CorrectAnswers,
    //         QuestionType.MultipleChoice => ((MultipleChoiceQuestion)CurrentQuestion).CorrectAlternativeIndices,
    //         _ => throw new InvalidOperationException("Unsupported question type.")
    //     };
    // }
}
