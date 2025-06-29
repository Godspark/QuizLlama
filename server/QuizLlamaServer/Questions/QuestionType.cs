using System.Text.Json.Serialization;

namespace QuizLlamaServer.Questions;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum QuestionType
{
    MultipleChoice = 1,
    TrueFalse = 2,
    TypeAnswer = 3
}