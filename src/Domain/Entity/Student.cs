using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Enum;

namespace Domain.Entity
{
    public class Student
    {
        [Key]
        public Guid Id  { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public Gender Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public bool Status { get; set; }
        public DateTime CreateAt { get; set; }
        public virtual ICollection<Result> Results { get; set; } = null!;
        public virtual ICollection<StudentSelected> StudentSelects { get; set; } = null!;
        public Guid HighSchoolId { get; set; }
    }
}
