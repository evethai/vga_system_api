using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
    public class PersonalTest : BaseEntity
    {
        public Guid TestTypeId { get; set; }
        public TestType TestType { get; set; } = null!;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool Status { get; set; }
        public DateTime CreateAt { get; set; }
        public virtual ICollection<StudentTest> StudentTests { get; set; } = null!;
        public virtual ICollection<TestQuestion> TestQuestions { get; set; } = null!;
    }

}
