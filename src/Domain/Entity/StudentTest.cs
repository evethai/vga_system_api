using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
    public class StudentTest
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public Guid StudentId { get; set; }
        public Student Student { get; set; } = null!;

        public int PersonalTestId { get; set; }
        public PersonalTest PersonalTest { get; set; } = null!;

        public int PersonalGroupId { get; set; }
        public PersonalGroup PersonalGroup { get; set; } = null!;

        public string JsonResult { get; set; } = string.Empty;
        public DateTime Date { get; set; }
    }

}
