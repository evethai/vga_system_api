using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Enum;
using Domain.Model.AccountWallet;
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
        public List<UniversityLocationModel> LocationUniversity { get; set; }
    }
    public class ResponseUniversityModel
    {
        public int? total { get; set; }
        public int? currentPage { get; set; }
        public List<UniversityModel> _universities { get; set; }
    }
}
