using System.Text.Json.Serialization;
using Domain.Enum;

namespace Domain.Model.Question
{
    public class AnswerModel
    {
        public int Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public AnswerValue AnswerValue { get; set; }
    }
}
