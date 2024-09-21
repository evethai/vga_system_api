using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model.Student;
public class StudentPutModel
{
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public bool Status { get; set; }   
}
