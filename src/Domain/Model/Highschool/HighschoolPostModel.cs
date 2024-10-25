using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Enum;
using Domain.Model.Account;

namespace Domain.Model.Highschool;
public class HighschoolPostModel : AccountPostModel
{
    [Required(ErrorMessage = "Address is required.")]
    public string Address { get; set; }
    [Required(ErrorMessage = "RegionId is required.")]
    public Guid RegionId { get; set; }
}
