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
    public class Answer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public PersonalityTypeMBTI PersonalityType { get; set; } 
        public int QuestionId { get; set; }
        public Question Question { get; set; } = null!;
        public virtual ICollection<StudentSelected> StudentSelects { get; set; } = null!;
    }
}
