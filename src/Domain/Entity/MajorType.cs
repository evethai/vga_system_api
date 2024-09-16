using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
    public class MajorType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int PersonalGroupId { get; set; }
        public PersonalGroup PersonalGroup { get; set; } = null!;
        public int MajorId { get; set; }
        public Major Major { get; set; } = null!;
        public bool status { get; set; }
    }
}
