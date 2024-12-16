using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
    public class MajorPersonalityMatrix : BasicEntity
    {
        public Guid PersonalGroupId { get; set; }
        public virtual PersonalGroup PersonalGroup { get; set; } = null!;
        public Guid MajorCategoryId { get; set; }
        public virtual MajorCategory MajorCategory { get; set; } = null!;
        public int AppropriateLevel { get; set; }
    }
}
