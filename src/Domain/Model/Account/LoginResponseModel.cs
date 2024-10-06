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
        public Guid StudentId { get; set; }

        public StudentAccountResponseModel(RoleEnum role,string name, Guid studentId)
            : base(role,name) 
        {
            StudentId = studentId;
        }
    }

    public class CareerExpertAccountResponseModel : LoginResponseModel
    {
        public Guid CareerExpertId { get; set; }

        public CareerExpertAccountResponseModel(RoleEnum role,string name, Guid careerExpertId)
            : base(role,name) 
        {
            CareerExpertId = careerExpertId;
        }
    }

    public class HighSchoolAccountResponseModel : LoginResponseModel
    {
        public Guid HighSchoolId { get; set; }

        public HighSchoolAccountResponseModel(RoleEnum role, string name, Guid highSchoolId) : base(role, name) 
        {
            HighSchoolId = highSchoolId;
        }
    }

    public class UniversityAccountResponseModel : LoginResponseModel
    {
        public Guid UniversityId { get; set; }

        public UniversityAccountResponseModel(RoleEnum role, string name, Guid universityId) : base(role, name) 
        {
            UniversityId = universityId;
        }
    }


}
