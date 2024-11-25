using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entity;
using Domain.Enum;

namespace Domain.Model.Question
{
    public class QuestionModel
    {
        public int Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public QuestionGroup Group { get; set; }
        public List<AnswerModel>? Answers { get; set; }
    }

    public class  QuestionListByTestIdModel
    {
        public int QuestionId { get; set; }
        public string Content { get; set; } = string.Empty;
        public QuestionGroup Group { get; set; }
        public List<AnswerModel>? AnswerModels { get; set; }
    }
}
