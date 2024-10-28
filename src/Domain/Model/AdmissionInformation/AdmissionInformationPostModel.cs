using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model.AdmissionInformation
{
    public class AdmissionInformationPostModel
    {
        [Required(ErrorMessage = "MajorId is required.")]
        public Guid MajorId { get; set; }
        [Required(ErrorMessage = "AdmissionMethodId is required.")]
        public Guid AdmissionMethodId { get; set; }
        [Range(0, double.MaxValue, ErrorMessage = "TuitionFee must be a positive number.")]
        public double TuitionFee { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "Year must be a positive number.")]
        public int Year { get; set; }
        [Range(0, int.MaxValue, ErrorMessage = "QuantityTarget must be a positive number.")]
        public int QuantityTarget { get; set; }
    }
}
