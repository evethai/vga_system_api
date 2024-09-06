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
    public class Question
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public OppositePairMBTI Type { get; set; } 
        public int TestId { get; set; }
        public Test Test { get; set; } = null!;
        public bool Status { get; set; }
        public DateTime CreateAt { get; set; }
        public virtual ICollection<Answer> Answers { get; set; } = null!;


    }
}
