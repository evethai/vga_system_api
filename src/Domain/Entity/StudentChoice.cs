using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
    public class StudentChoice : BasicEntity
    {
        public Guid StudentId { get; set; }
        public virtual Student Student { get; set; } = null!;   
        public Guid MajorId { get; set; }
        public Guid OccupationId { get; set; }
        public int MajorVote { get; set; }
        public int OccupationVote { get; set; }
        public DateTime CreateAt { get; set; }
        public bool Status { get; set; }
    }
}
