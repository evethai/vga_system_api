using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Domain.Enum;
using Domain.Model.Account;

namespace Domain.Model.Student;
public class StudentJsonModel : AccountPostModel
{
    //[Required(ErrorMessage = "Name is required.")]
    //public string Name { get; set; } = string.Empty;
    [Required(ErrorMessage = "Gender is required.")]
    public bool Gender { get; set; }
    [Required(ErrorMessage = "excelSerialDate is required.")]
    public double excelSerialDate { get; set; }
    [JsonIgnore]
    public bool Status { get; set; } = true;
    [JsonIgnore]
    public DateTime CreateAt { get; set; }  = DateTime.UtcNow.AddHours(7);
}
