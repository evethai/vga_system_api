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
    public class TestType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public TestTypeCode TypeCode { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool Status { get; set; }
        public DateTime CreateAt { get; set; }
        public virtual ICollection<PersonalTest> PersonalTests { get; set; } = null!;
        public virtual ICollection<PersonalGroup> PersonalGroups { get; set; } = null!;
        public virtual ICollection<Question> Questions { get; set; } = null!;

    }
}
