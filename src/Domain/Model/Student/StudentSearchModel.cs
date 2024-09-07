using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Domain.Model.Student;
public class StudentSearchModel
{
    [FromQuery(Name = "current-page")]
    public int? currentPage { get; set; }
    [FromQuery(Name = "page-size")]
    public int? pageSize { get; set; }
    [FromQuery(Name = "name")]
    public string? name { get; set; }
    [FromQuery(Name = "highschool-id")]
    public int? highschoolId { get; set; }
    [FromQuery(Name = "sort-by-gold")]
    public bool? sortByGold { get; set; } = false;
    [FromQuery(Name = "descending")]
    public bool? descending { get; set; } = false;
}
