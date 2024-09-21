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
        public Guid TestTypeId { get; set; }
        public QuestionGroup Group { get; set; }
        public List<AnswerModel>? _answerModels { get; set; }
    }
}
