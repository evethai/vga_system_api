using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Domain.Enum;

namespace Domain.Model.Student;
public class StudentImportModel
{
    [Required(ErrorMessage = "String Json is required.")]
    public string stringJson { get; set; } = string.Empty;
    [Required(ErrorMessage = "High school Id is required.")]
    public Guid highschoolId { get; set; }

    [Required(ErrorMessage = "School year is required.")]
    public int schoolYear { get; set; }
}
