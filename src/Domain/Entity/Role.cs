using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
    public class Role : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public virtual ICollection<Account> Accounts { get; set; } = new List<Account>();
    }
}
