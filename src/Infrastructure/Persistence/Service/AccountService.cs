using System.Linq.Expressions;
using System.Security.Cryptography;
using Application.Common.Utils;
using Application.Interface;
using Application.Interface.Service;
using Domain.Entity;
using Domain.Enum;
using Domain.Model.Account;
using Domain.Model.Response;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

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
        #region Login
        public async Task<LoginResponseModel> Login(LoginRequestModel loginRequest)
        {
            Expression<Func<Account, bool>> searchFilter = x =>
            x.Email.Equals(loginRequest.Email) &&
            x.Password.Equals(PasswordUtil.HashPassword(loginRequest.Password));

            Account account = await _unitOfWork.AccountRepository
                .SingleOrDefaultAsync(predicate: searchFilter);

            if (account == null 
                || account.Status == AccountStatus.Blocked
                || account.Role == RoleEnum.Consultant) return null;

            RoleEnum role = account.Role;

            return await BuildLoginResponse(account, role);
        }
        #endregion

        #region Login By Zalo
        public async Task<LoginResponseModel> LoginByZalo(ZaloLoginModel model)
        {
            Account account = await _unitOfWork.AccountRepository
                .SingleOrDefaultAsync(predicate: x => x.ZaloId.Equals(model.ZaloId));


            if (account != null && account.Status == AccountStatus.Active)
            {
                RoleEnum role = account.Role;
                if (role == RoleEnum.Admin
                    || role == RoleEnum.HighSchool
                    || role == RoleEnum.University
                    || account.Status == AccountStatus.Blocked) return null;
                return await BuildLoginResponse(account, role);
            }
            if (!string.IsNullOrEmpty(model.Phone))
            {
                account = await _unitOfWork.AccountRepository
                    .SingleOrDefaultAsync(predicate: x => x.Phone.Equals(model.Phone));

                if (account != null && account.Status == AccountStatus.Active)
                {

                    account.ZaloId = model.ZaloId;
                    account.Image_Url = model.Image_Url;
                    await _unitOfWork.AccountRepository.UpdateAsync(account);
                    await _unitOfWork.SaveChangesAsync();

                    RoleEnum role = account.Role;
                    if (role == RoleEnum.Admin
                        || role == RoleEnum.HighSchool
                        || role == RoleEnum.University
                        || account.Status == AccountStatus.Blocked) return null;
                    return await BuildLoginResponse(account, role);
                }
            }
            return null;
        }
        #endregion

        #region Logout
        public async Task<ResponseModel> Logout(Guid AccountId)
        {
            try
            {
                var existingRefreshToken = await _unitOfWork.RefreshTokenRepository
                    .SingleOrDefaultAsync(predicate: x => x.AccountId == AccountId && !x.IsRevoked);

                if (existingRefreshToken != null)
                {
                    existingRefreshToken.IsRevoked = true;
                    await _unitOfWork.RefreshTokenRepository.UpdateAsync(existingRefreshToken);
                    await _unitOfWork.SaveChangesAsync();

                    return new ResponseModel
                    {
                        IsSuccess = true,
                        Message = "Logout successful, Token has been revoked"
                    };
                }

                return new ResponseModel
                {
                    IsSuccess = false,
                    Message = "No active refresh token found for this account."
                };
            }
            catch (Exception ex)
            {
                return new ResponseModel
                {
                    IsSuccess = false,
                    Message = "An error occurred while revoking the token."
                };
            }
        }
        #endregion

        #region Refresh Token
        public async Task<ResponseModel> CreateRefreshToken(RefreshTokenRequestModel model)
        {
            var existingRefreshToken = await _unitOfWork.RefreshTokenRepository
                .SingleOrDefaultAsync(predicate: x => x.AccountId == model.AccountId && !x.IsRevoked);

            if (existingRefreshToken == null)
            {
                return new ResponseModel
                {
                    IsSuccess = false,
                    Message = "This refresh token does not exist"
                };
            }

            if (existingRefreshToken.IsUsed)
            {
                return new ResponseModel
                {
                    IsSuccess = false,
                    Message = "Refresh token has been used"
                };
            }

            if (existingRefreshToken.IsRevoked)
            {
                return new ResponseModel
                {
                    IsSuccess = false,
                    Message = "Refresh token has been revoked"
                };
            }

            if (existingRefreshToken.ExpireAt > DateTime.UtcNow)
            {
                return new ResponseModel
                {
                    IsSuccess = false,
                    Message = "This token hasn't expired yet"
                };
            }


            existingRefreshToken.IsUsed = true;
            existingRefreshToken.IsRevoked = true;
            await _unitOfWork.SaveChangesAsync();


            var account = await _unitOfWork.AccountRepository
                .SingleOrDefaultAsync(predicate: x => x.Id == model.AccountId);

            if (account == null)
            {
                return new ResponseModel
                {
                    IsSuccess = false,
                    Message = "Account not found"
                };
            }

            RoleEnum role = account.Role;
            var responseModel = await BuildLoginResponse(account, role);

            return new ResponseModel
            {
                IsSuccess = true,
                Message = "Create new token success",
                Data = responseModel
            };
        }
        #endregion

        #region Build Reponse Model
        private async Task<LoginResponseModel> BuildLoginResponse(Account account, RoleEnum role)
        {
            Tuple<string, Guid> guidClaim = null;
            LoginResponseModel loginResponseModel = null;
            string? PichUrl = null;
            string name = await _unitOfWork.AccountRepository.SingleOrDefaultAsync(selector: x=> x.Name, predicate: x=>x.Id.Equals(account.Id));
            switch (role)
            {
                case RoleEnum.Student:
                    Guid studentId = await _unitOfWork.StudentRepository
                        .SingleOrDefaultAsync(selector: x => x.Id, predicate: x => x.AccountId.Equals(account.Id));
                    guidClaim = new Tuple<string, Guid>("StudentId", studentId);
                    loginResponseModel = new StudentAccountResponseModel(account.Id,role,name,studentId);
                    break;

                case RoleEnum.Consultant:
                    Guid careerExpertId = await _unitOfWork.ConsultantRepository
                        .SingleOrDefaultAsync(selector: x => x.Id, predicate: x => x.AccountId.Equals(account.Id));
                    guidClaim = new Tuple<string, Guid>("CareerExpertId", careerExpertId);
                    loginResponseModel = new CareerExpertAccountResponseModel(account.Id,role,name,careerExpertId);
                    break;

                case RoleEnum.HighSchool:
                    Guid highSchoolId = await _unitOfWork.HighschoolRepository
                        .SingleOrDefaultAsync(selector: x => x.Id, predicate: x => x.AccountId.Equals(account.Id));
                    guidClaim = new Tuple<string, Guid>("HighSchoolId", highSchoolId);
                    loginResponseModel = new HighSchoolAccountResponseModel(account.Id, role,name, highSchoolId);
                    break;

                case RoleEnum.University:
                    Guid universityId = await _unitOfWork.UniversityRepository
                        .SingleOrDefaultAsync(selector: x => x.Id, predicate: x => x.AccountId.Equals(account.Id));
                    //name = await _unitOfWork.UniversityRepository
                    //    .SingleOrDefaultAsync(selector: x => x.Name, predicate: x => x.AccountId.Equals(account.Id));
                    guidClaim = new Tuple<string, Guid>("UniversityId", universityId);
                    loginResponseModel = new UniversityAccountResponseModel(account.Id,role, name, universityId);
                    break;

                default:
                    guidClaim = new Tuple<string, Guid>("Admin", account.Id);
                    loginResponseModel = new LoginResponseModel(account.Id,role,name);
                    break;
            }

            var jwt = JwtUtil.GenerateJwtToken(account, guidClaim, _configuration);

            var existingRefreshToken = await _unitOfWork.RefreshTokenRepository
                .SingleOrDefaultAsync(predicate: x => x.AccountId == account.Id && !x.IsRevoked && !x.IsUsed);

            string refreshToken;

            if (existingRefreshToken != null && existingRefreshToken.ExpireAt > DateTime.UtcNow)
            {
                refreshToken = existingRefreshToken.Token;
            }
            else
            {

                refreshToken = GenerateRefreshTokenString();

                RefreshToken r_token = new RefreshToken
                {
                    Id = Guid.NewGuid(),
                    AccountId = account.Id,
                    Token = refreshToken,
                    JwtId = jwt.Id,
                    IsUsed = false,
                    IsRevoked = false,
                    ExpireAt = DateTime.UtcNow.AddDays(3),
                    IssuedAt = DateTime.UtcNow
                };

                await _unitOfWork.RefreshTokenRepository.AddAsync(r_token);
            }

            loginResponseModel.AccessToken = jwt.Token;
            loginResponseModel.RefreshToken = refreshToken;

            await _unitOfWork.SaveChangesAsync();
            return loginResponseModel;
        }
        #endregion
        private string GenerateRefreshTokenString()
        {
            var randomNumber = new byte[32];

            using (var numberGenerator = RandomNumberGenerator.Create())
            {
                numberGenerator.GetBytes(randomNumber);
            }

            return Convert.ToBase64String(randomNumber);
        }

        #region update status account
        public async Task<ResponseModel> UpdateStatusAccount(Guid accountId, AccountStatus status)
        {
            try
            {
                var account = await _unitOfWork.AccountRepository.SingleOrDefaultAsync(predicate: x => x.Id == accountId);
                if (account == null)
                {
                    return new ResponseModel
                    {
                        IsSuccess = false,
                        Message = "Account not found"
                    };
                }
                account.Status = status;
                await _unitOfWork.AccountRepository.UpdateAsync(account);
                await _unitOfWork.SaveChangesAsync();
                return new ResponseModel
                {
                    IsSuccess = true,
                    Message = "Update status account success"
                };
            }
            catch (Exception ex)
            {
                return new ResponseModel
                {
                    IsSuccess = false,
                    Message = "An error occurred while updating status account"
                };
            }
        }
        #endregion

    }
}
