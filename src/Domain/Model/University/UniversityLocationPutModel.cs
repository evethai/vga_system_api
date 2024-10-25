using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model.University
{
    public class UniversityLocationPutModel
    {
        [Required(ErrorMessage = "RegionId is required.")]
        public Guid RegionId { get; set; }
        [Required(ErrorMessage = "Address is required.")]
        public string Address { get; set; }
    }
}
