using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
    public class Like : BasicEntity
    {
        public Guid NewsId { get; set; }
        public News News { get; set; } = null!;
        public Guid AccountId { get; set; }
        public Account Account { get; set; } = null!;
        public bool Status { get; set; }
    }
}
