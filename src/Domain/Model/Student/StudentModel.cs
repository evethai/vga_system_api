using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Enum;
using Domain.Model.Highschool;

namespace Domain.Model.Student;
public class StudentModel
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public Gender Gender { get; set; }
    public DateTime DateOfBirth { get; set; }
    public bool Status { get; set; }
    public DateTime CreateAt { get; set; }
    public Guid HighSchoolId { get; set; }
    public int GoldBalance { get; set; }
}
public class ResponseStudentModel
{
    public int? total { get; set; }
    public int? currentPage { get; set; }
    public List<StudentModel> students { get; set; }
}
