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
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; } 
        public string Name { get; set; }
        public RoleEnum Role { get; set; }


        public LoginResponseModel( RoleEnum role, string name)
        {
            Role = role;
            Name = name;

        }
    }

    public class StudentAccountResponseModel : LoginResponseModel
    {
        public Guid UserId { get; set; }

        public StudentAccountResponseModel(RoleEnum role,string name, Guid studentId)
            : base(role,name) 
        {
            UserId = studentId;
        }
    }

    public class CareerExpertAccountResponseModel : LoginResponseModel
    {
        public Guid UserId { get; set; }

        public CareerExpertAccountResponseModel(RoleEnum role, string name, Guid careerExpertId)
            : base(role, name) 
        {
            UserId = careerExpertId;
        }
    }

    public class HighSchoolAccountResponseModel : LoginResponseModel
    {
        public Guid UserId { get; set; }

        public HighSchoolAccountResponseModel(RoleEnum role, string name, Guid highSchoolId) : base(role, name) 
        {
            UserId = highSchoolId;
        }
    }

    public class UniversityAccountResponseModel : LoginResponseModel
    {
        public Guid UserId { get; set; }

        public UniversityAccountResponseModel(RoleEnum role, string name, Guid universityId) : base(role, name) 
        {
            UserId = universityId;
        }
    }


}
