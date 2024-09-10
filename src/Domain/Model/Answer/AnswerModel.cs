using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Domain.Enum;

namespace Domain.Model.Answer
{
    public class AnswerModel
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
        [JsonPropertyName("content")]
        public string Content { get; set; } = string.Empty;
        [JsonPropertyName("personalityType")]
        public PersonalityTypeMBTI PersonalityType { get; set; }
        [JsonPropertyName("questionId")]
        public int QuestionId { get; set; }
    }
}
