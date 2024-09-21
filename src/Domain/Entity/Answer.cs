using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Enum;

namespace Domain.Entity
{
    public class Answer : BasicEntity
    {

        public int QuestionId { get; set; }
        public Question Question { get; set; } = null!;
        public string Content { get; set; } = string.Empty;
        public AnswerValue AnswerValue { get; set; } 
        public bool status { get; set; }
    }
}
