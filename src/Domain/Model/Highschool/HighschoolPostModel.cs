using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Enum;

namespace Domain.Model.Highschool;
public class HighschoolPostModel
{
    public string Name { get; set; }
    public string LocationDetail { get; set; }
    public Guid RegionId { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty!;
    public string Phone { get; set; } = string.Empty;   
}
