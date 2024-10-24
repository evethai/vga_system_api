using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
    public class StudentTest : BaseEntity
    {

        public Guid StudentId { get; set; }
        public Student Student { get; set; } = null!;

        public Guid PersonalTestId { get; set; }
        public PersonalTest PersonalTest { get; set; } = null!;

        public Guid PersonalGroupId { get; set; }
        public PersonalGroup PersonalGroup { get; set; } = null!;

        public string JsonResult { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public virtual ICollection<StudentChoice> StudentChoices { get; set; } = null!;
    }

}
