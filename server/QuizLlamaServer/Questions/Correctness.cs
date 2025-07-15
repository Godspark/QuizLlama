using System.Text.Json.Serialization;

namespace QuizLlamaServer.Questions;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum Correctness
{
    Correct = 1,
    Incorrect = 2,
    PartiallyCorrect = 3,
    NotAnswered = 4
}
