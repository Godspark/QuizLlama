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
    
    public int PlayerAnswered(Player player, Guess guess)
    {
        var correctness = CurrentQuestion.CheckAnswer(guess);
        var answer = new Answer
        {
            Question = CurrentQuestion,
            Player = player,
            Correctness = correctness,
            PointsAwarded = correctness == Correctness.Correct ? CurrentQuestion.MaxPoints : 0
        };

        switch (CurrentQuestion.QuestionType)
        {
            case QuestionType.MultipleChoice:
                answer.MultipleChoiceIndex = guess.MultipleChoiceIndex ?? -1;
                break;
            case QuestionType.TrueFalse:
                answer.TrueFalse = guess.TrueFalse ?? false;
                break;
            case QuestionType.TypeAnswer:
                answer.TypeAnswerText = guess.TypeAnswerText ?? string.Empty;
                break;
            default:
                throw new InvalidOperationException("PlayerAnswered(): Unsupported question type.");
        }
        
        Answers.Add(answer);
        player.Score += answer.PointsAwarded;
        return Interlocked.Increment(ref _playersAnsweredCount);
    }

    public Correctness GetCorrectness(Player player)
    {
        var answer = Answers.FirstOrDefault(x => x.Player == player && x.Question == CurrentQuestion);
        return answer?.Correctness ?? Correctness.NotAnswered;
    }

    public Solution GetCorrectAnswers()
    {
        var solution = new Solution();

        switch (CurrentQuestion.QuestionType)
        {
            case QuestionType.TrueFalse:
                solution.TrueFalseSolution = ((TrueFalseQuestion)CurrentQuestion).CorrectAnswer;
                break;
            case QuestionType.TypeAnswer:
                solution.TypeAnswerSolutions = ((TypeAnswerQuestion)CurrentQuestion).CorrectAnswers;
                break;
            case QuestionType.MultipleChoice:
                solution.MultipleChoiceSolutionIndices = ((MultipleChoiceQuestion)CurrentQuestion).CorrectAlternativeIndices;
                break;
            default:
                throw new InvalidOperationException("GetCorrectAnswers(): Unsupported question type.");
        }
        
        return solution;
    }
    
    public AnswerDistribution GetAnswerDistribution()
    {
        var answerDistribution = new AnswerDistribution();
        
        switch (CurrentQuestion.QuestionType)
        {
            case QuestionType.TrueFalse:
                answerDistribution.TrueFalseDistribution.Add(true, Answers.Count(a => a.TrueFalse));
                answerDistribution.TrueFalseDistribution.Add(false, Answers.Count(a => !a.TrueFalse));
                break;
            case QuestionType.TypeAnswer:
                foreach (var answer in Answers)
                {
                    if (!answerDistribution.TypeAnswerDistribution.TryAdd(answer.TypeAnswerText, 1))
                    {
                        answerDistribution.TypeAnswerDistribution[answer.TypeAnswerText]++;
                    }
                }
                break;
            case QuestionType.MultipleChoice:
                foreach (var answer in Answers)
                {
                    if (!answerDistribution.MultipleChoiceDistribution.TryAdd(answer.MultipleChoiceIndex, 1))
                    {
                        answerDistribution.MultipleChoiceDistribution[answer.MultipleChoiceIndex]++;
                    }
                }
                break;
            default:
                throw new InvalidOperationException("GetAnswerDistribution(): Unsupported question type.");
        }

        return answerDistribution;
    }

    public Scoreboard GetScoreboard()
    {
        return new Scoreboard();
    }
}
