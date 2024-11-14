using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Enum;

namespace Domain.Model.Account
{
    public class LoginResponseModel
    {
        public Guid AccountId { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; } 
        public string Name { get; set; }
        public RoleEnum Role { get; set; }


        public LoginResponseModel(Guid accountId, RoleEnum role, string name)
        {
            AccountId = accountId;
            Role = role;
            Name = name;

        }
    }

    public class StudentAccountResponseModel : LoginResponseModel
    {
        public Guid UserId { get; set; }

        public StudentAccountResponseModel(Guid accountId, RoleEnum role,string name, Guid studentId)
            : base(accountId,role,name) 
        {
            UserId = studentId;
        }
    }

    public class CareerExpertAccountResponseModel : LoginResponseModel
    {
        public Guid UserId { get; set; }

        public CareerExpertAccountResponseModel(Guid accountId, RoleEnum role, string name, Guid careerExpertId)
            : base(accountId, role, name) 
        {
            UserId = careerExpertId;
        }
    }

    public class HighSchoolAccountResponseModel : LoginResponseModel
    {
        public Guid UserId { get; set; }

        public HighSchoolAccountResponseModel(Guid accountId, RoleEnum role, string name, Guid highSchoolId) : base(accountId, role, name) 
        {
            UserId = highSchoolId;
        }
    }

    public class UniversityAccountResponseModel : LoginResponseModel
    {
        public Guid UserId { get; set; }

        public UniversityAccountResponseModel(Guid accountId, RoleEnum role, string name, Guid universityId) : base(accountId, role, name) 
        {
            UserId = universityId;
        }
    }


}
