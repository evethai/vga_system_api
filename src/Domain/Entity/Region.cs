using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity;
public class Region : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string DOET { get; set; } = null!;
    public bool Status { get; set; }
    public virtual ICollection<HighSchool> HighSchools { get; set; } = null!;
    public virtual ICollection<UniversityLocation> UniversityLocations { get; set; } = null!;
}
