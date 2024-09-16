using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
    public class PersonalGroup
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int TestTypeId { get; set; }
        public TestType TestType { get; set; } = null!;
        public bool status { get; set; }
        public virtual ICollection<MajorType> MajorTypes { get; set; } = null!;
        public virtual ICollection<StudentTest> StudentTests { get; set; } = null!;



    }
}
