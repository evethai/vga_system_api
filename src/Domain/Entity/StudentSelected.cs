using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
    public class StudentSelected
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public Guid StudentId { get; set; }
        public Student Student { get; set; } = null!;
        public int AnswerId { get; set; }
        public Answer Answer { get; set; } = null!;
        public DateTime CreateAt { get; set; }
    }
}
