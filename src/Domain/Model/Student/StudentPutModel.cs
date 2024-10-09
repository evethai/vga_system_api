using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Model.Account;

namespace Domain.Model.Student;
public class StudentPutModel : AccountPostModel
{
    [Required(ErrorMessage = "Name is required.")]
    public string Name { get; set; } = string.Empty;
    [Required(ErrorMessage = "Gender is required.")]
    public bool Gender { get; set; }
    [Required(ErrorMessage = "DateOfBirth is required.")]
    [DataType(DataType.DateTime)]
    public DateTime DateOfBirth { get; set; }
    [Required(ErrorMessage = "SchoolYears is required.")]
    public int SchoolYears { get; set; }
}
