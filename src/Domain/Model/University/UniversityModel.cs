using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Enum;
using Domain.Model.AccountWallet;
using Domain.Model.AdmissionInformation;
using Domain.Model.Consultant;
using Domain.Model.ConsultantLevel;
using Domain.Model.Highschool;

namespace Domain.Model.University
{
    public class UniversityModel
    {
        public Guid Id { get; set; }
        public string Description { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string EstablishedYear { get; set; } = string.Empty;
        public UniversityType Type { get; set; }
        public AccountWalletModel Account { get; set; }
        public List<UniversityLocationModel> UniversityLocations { get; set; }
    }
    public class UniversityModelGetBy
    {
        public Guid Id { get; set; }
        public string Description { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string EstablishedYear { get; set; } = string.Empty;
        public UniversityType Type { get; set; }
        public AccountWalletModel Account { get; set; }
        public List<UniversityLocationModel> UniversityLocations { get; set; }
        public List<ConsultantModel> Consultants { get; set; }
        public List<AdmissionInformationModel> AdmissionInformation { get; set; }
    }
    public class ConsultantModel
    {
        public Guid Id { get; set; }
        public Guid AccountId { get; set; }
        public string Name { get; set; } = string.Empty;
        public ConsultantLevelViewModel ConsultantLevel { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string? Image_Url { get; set; }
        public string Description { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public bool Gender { get; set; }
        public DateTime CreateAt { get; set; }
    }
    public class ResponseUniversityModel
    {
        public int? total { get; set; }
        public int? currentPage { get; set; }
        public List<UniversityModel> _universities { get; set; }
    }
}
