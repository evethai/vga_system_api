using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Model.Account;
using Domain.Model.Certification;
using Domain.Model.ConsultantRelation;

namespace Domain.Model.Consultant
{
    public class ConsultantPutModel : AccountPutModel
    {
        [Required(ErrorMessage = "Consultant level id is required.")]
        public int ConsultantLevelId { get; set; }
        //[Required(ErrorMessage = "University Id level id is required.")]
        //public Guid UniversityId { get; set; }
        [Required(ErrorMessage = "Description is required.")]
        public string Description { get; set; } = string.Empty;
        [Required(ErrorMessage = "DateOfBirth is required.")]
        public DateTime DoB { get; set; }
        [Required(ErrorMessage = "Gender is required.")]
        public bool Gender { get; set; }
        public string? Image_Url { get; set; }
        public List<CertificationPutModel>? Certifications { get; set; }
        public List<ConsultantRelationPostPutModel>? ConsultantRelations { get; set; }
    }
}
