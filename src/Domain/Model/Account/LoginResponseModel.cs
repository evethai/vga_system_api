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
        public Guid Id { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Name { get; set; }
        public RoleEnum Role { get; set; }
        public AccountStatus Status { get; set; }
        public string? BrandPicUrl { get; set; }
        
        public LoginResponseModel(Guid id, string email, string phone, string name, RoleEnum role, AccountStatus status, string? brandPicUrl)
        {
            Id = id;
            Email = email;
            Phone = phone;
            Name = name;
            Role = role;
            Status = status;
            BrandPicUrl = brandPicUrl;
        }

    }
    public class StudentAccountResponseModel : LoginResponseModel
    {
        public Guid StudentId { get; set; }
        public StudentAccountResponseModel(Guid id, string email, string phone, string name, RoleEnum role, AccountStatus status, string? brandPicUrl, Guid studentId) : base(id, email, phone, name, role, status, brandPicUrl)
        {
            StudentId = studentId;
        }
    }
    public class CareerExpertAccountResponseModel : LoginResponseModel
    {
        public Guid CareerExpertId { get; set; }
        public CareerExpertAccountResponseModel(Guid id, string email, string phone, string name, RoleEnum role, AccountStatus status, string? brandPicUrl, Guid careerExpertId) : base(id, email, phone, name, role, status, brandPicUrl)
        {
            CareerExpertId = careerExpertId;
        }
    }

    public class HighSchoolAccountResponseModel : LoginResponseModel
    {
        public Guid HighSchoolId { get; set; }
        public HighSchoolAccountResponseModel(Guid id, string email, string phone, string name, RoleEnum role, AccountStatus status, string? brandPicUrl, Guid highSchoolId) : base(id, email, phone, name, role, status, brandPicUrl)
        {
            HighSchoolId = highSchoolId;
        }
    }

    public class UniversityAccountResponseModel : LoginResponseModel
    {
        public Guid UniversityId { get; set; }
        public UniversityAccountResponseModel(Guid id, string email, string phone, string name, RoleEnum role, AccountStatus status, string? brandPicUrl, Guid universityId) : base(id, email, phone, name, role, status, brandPicUrl)
        {
            UniversityId = universityId;
        }
    }

}
