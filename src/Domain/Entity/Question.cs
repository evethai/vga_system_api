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
    public class Question : BaseEntity
    {
        public int TestTypeId { get; set; }
        public TestType TestType { get; set; } = null!;
        public string Content { get; set; } = string.Empty;
        public QuestionGroup Group { get; set; }
        public bool Status { get; set; }
        public DateTime CreateAt { get; set; }
        public virtual ICollection<Answer> Answers { get; set; } = null!;
        public virtual ICollection<TestQuestion> TestQuestions { get; set; } = null!;
    }

}
