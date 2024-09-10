using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity;
public class HighSchool
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    public string Name { get; set; }
    public int GoldBalance { get; set; }
    public string LocationDetails { get; set; }
    public string ContactInfor { get; set; }
    public virtual ICollection <Student> Students { get; set; } = null!;
    public int RegionId { get; set; }
}
