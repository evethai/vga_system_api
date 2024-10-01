using System.Linq.Expressions;
using Application.Common.Utils;
using Application.Interface;
using Application.Interface.Service;
using Domain.Entity;
using Domain.Enum;
using Domain.Model.Account;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Persistence.Service
{
    public class AccountService : IAccountService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        public AccountService(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }

        public async Task<LoginResponseModel> Login(LoginRequestModel loginRequest)
        {
            Expression<Func<Account, bool>> searchFilter = x =>
            x.Email.Equals(loginRequest.Email) &&
            x.Password.Equals(PasswordUtil.HashPassword(loginRequest.Password));
            Account account = await _unitOfWork.AccountRepository
                .SingleOrDefaultAsync(predicate: searchFilter, include: x => x.Include(x => x.Role));
            if (account == null) return null;
            RoleEnum role = EnumUtil.ParseEnum<RoleEnum>(account.Role.Name);
            Tuple<string, Guid> guidClaim = null;
            LoginResponseModel loginResponseModel = null;
            string? brandPicUrl = null;

            switch(role)
            {
                case RoleEnum.Student:
                    Guid studentId = await _unitOfWork.StudentRepository
                        .SingleOrDefaultAsync(selector: x => x.Id, predicate: x => x.AccountId.Equals(account.Id));
                    guidClaim = new Tuple<string, Guid>("StudentId", studentId);
                    loginResponseModel = new StudentAccountResponseModel(account.Id, account.Email, account.Phone, account.Student.Name, role, account.Status, brandPicUrl, studentId);
                    break;
                case RoleEnum.Expert:
                    Guid careerExpertId = await _unitOfWork.CareerExpertRepository
                        .SingleOrDefaultAsync(selector: x => x.Id, predicate: x => x.AccountId.Equals(account.Id));
                    guidClaim = new Tuple<string, Guid>("CareerExpertId", careerExpertId);
                    loginResponseModel = new CareerExpertAccountResponseModel(account.Id, account.Email, account.Phone, account.CareerExpert.Name, role, account.Status, brandPicUrl, careerExpertId);
                    break;
                case RoleEnum.HighSchool:
                    Guid highSchoolId = await _unitOfWork.HighschoolRepository
                        .SingleOrDefaultAsync(selector: x => x.Id, predicate: x => x.AccountId.Equals(account.Id));
                    guidClaim = new Tuple<string, Guid>("HighSchoolId", highSchoolId);
                    loginResponseModel = new HighSchoolAccountResponseModel(account.Id, account.Email, account.Phone, account.HighSchool.Name, role, account.Status, brandPicUrl, highSchoolId);
                    break;
                case RoleEnum.University:
                    Guid universityId = await _unitOfWork.UniversityRepository
                        .SingleOrDefaultAsync(selector: x => x.Id, predicate: x => x.AccountId.Equals(account.Id));
                    guidClaim = new Tuple<string, Guid>("UniversityId", universityId);
                    loginResponseModel = new UniversityAccountResponseModel(account.Id, account.Email, account.Phone, account.University.Name, role, account.Status, brandPicUrl, universityId);
                    break;
                default:
                    loginResponseModel = new LoginResponseModel(account.Id, account.Email, account.Phone, account.Role.Name, role, account.Status, brandPicUrl);
                    break;
            }
            var token = JwtUtil.GenerateJwtToken(account, guidClaim,_configuration);
            loginResponseModel.AccessToken = token;
            return loginResponseModel;
        }


       

    }
}
