using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model.Certification
{
    public class CertificationPutModel
    {
        public int? Id { get; set; }
        public string? Description { get; set; }
        public string? ImageUrl { get; set; } 
    }
}
