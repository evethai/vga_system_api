using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model.Highschool;
public class HighschoolPutModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public int GoldBalance { get; set; }
    public string LocationDetails { get; set; }
    public string ContactInfor { get; set; }
}
