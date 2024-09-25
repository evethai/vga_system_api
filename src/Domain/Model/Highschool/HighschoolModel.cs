using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entity;

namespace Domain.Model.Highschool;
public class HighschoolModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }   
    public string LocationDetails { get; set; }
    public Guid RegionId { get; set; }
    public Guid AccountId { get; set; }
}
public class ResponseHighSchoolModel
{
    public int? total { get; set; }
    public int? currentPage { get; set; }
    public List<HighschoolModel> highschools { get; set; }
}
