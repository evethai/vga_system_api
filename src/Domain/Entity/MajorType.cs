using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
    public class MajorType : BasicEntity
    {

        public Guid PersonalGroupId { get; set; }
        public PersonalGroup PersonalGroup { get; set; } = null!;
        public Guid MajorId { get; set; }
        public Major Major { get; set; } = null!;
        public bool Status { get; set; }
    }
}
