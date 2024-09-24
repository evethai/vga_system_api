using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model.Highschool;
public class HighschoolPostModel
{
    public string Name { get; set; }
    public string LocationDetails { get; set; }
    public Guid RegionId { get; set; }
    public Guid AccountId { get; set; }
}
