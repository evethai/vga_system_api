using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model.Region;
public class RegionModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string DOET { get; set; } = null!;
    public bool Status { get; set; }
}
public class ResponseRegionModel
{
    public List<RegionModel> regions { get; set; }
}
