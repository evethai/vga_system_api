using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
    public class MajorOccupationMatrix : BasicEntity
    {
        public Guid MajorCategoryId { get; set; }
        public virtual MajorCategory MajorCategory { get; set; } = null!;
        public Guid OccupationalGroupId { get; set; }
        public virtual OccupationalGroup OccupationalGroup { get; set; } = null!;
    }
}
