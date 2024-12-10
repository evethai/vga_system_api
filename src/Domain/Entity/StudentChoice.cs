using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Enum;

namespace Domain.Entity
{
    public class StudentChoice : BasicEntity
    {
        public Guid StudentId { get; set; }
        public virtual Student Student { get; set; } = null!;
        public Guid MajorOrOccupationId { get; set; }
        public string MajorOrOccupationName { get; set; } = null!;
        public int Rating { get; set; }
        public bool isMajor { get; set; }
        public StudentChoiceType Type { get; set; }
    }
}
