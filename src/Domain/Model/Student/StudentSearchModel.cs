using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Enum;
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
    public Guid? highschoolId { get; set; }
    [FromQuery(Name = "school-years")]
    public int? SchoolYears { get; set; }
    [FromQuery(Name = "status")]
    public AccountStatus? Status { get; set; }
    [FromQuery(Name = "descending")]
    public bool? descending { get; set; } = false;
}
