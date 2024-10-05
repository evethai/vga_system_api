using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model.Student;
public class StudentPutModel
{
    [Required(ErrorMessage = "Status is required.")]
    public bool Status { get; set; }   
}
