using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Enum;
using Domain.Model.Certification;
using Domain.Model.ConsultantLevel;
using Domain.Model.University;

namespace Domain.Model.Consultant
{
    public class ConsultantViewModel
    {
        public Guid Id { get; set; }
        public Guid AccountId { get; set; }
        public string Name { get; set; } = string.Empty;
        public ConsultantLevelViewModel ConsultantLevel { get; set; }
        public UniversityModel University { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string? Image_Url { get; set; }
        public string Description { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public bool Gender { get; set; }
        public AccountStatus AccountStatus { get; set; }
        public DateTime CreateAt { get; set; }
        public List<CertificationViewModel> Certifications { get; set; } 
    }
}
