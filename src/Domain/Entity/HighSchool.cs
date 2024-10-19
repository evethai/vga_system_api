using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Enum;

namespace Domain.Entity;
public class HighSchool : BaseEntity
{
    public Guid RegionId { get; set; }
    public Region Region { get; set; } = null!;
    public Guid AccountId { get; set; }
    public virtual Account Account { get; set; } = null!;
    //public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public virtual ICollection<Student> Students { get; set; } = null!;
}
  
