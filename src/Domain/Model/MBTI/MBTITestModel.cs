using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Domain.Enum;
using Newtonsoft.Json;

namespace Domain.Model.MBTI
{
    public class MBTITestModel
    {
        [JsonProperty("questionId")]
        public int QuestionId { get; set; }

        [JsonProperty("questionContent")]
        public string QuestionContent { get; set; } = null!;

        [JsonProperty("answer")]
        public List<MBTIAnswerModel> Answer { get; set; } = null!;
    }

    public class MBTIAnswerModel
    {
        [JsonProperty("answerId")]
        public int AnswerId { get; set; }

        [JsonProperty("answerContent")]
        public string AnswerContent { get; set; } = null!;

        [JsonProperty("personalityType")]
        public PersonalityTypeMBTI PersonalityType { get; set; }
    }

}
