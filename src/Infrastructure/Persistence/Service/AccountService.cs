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
        private readonly ILogger _logger;
        public AccountService(IUnitOfWork unitOfWork, IConfiguration configuration, ILogger logger)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
            _logger = logger;
        }
        #region Login
        public async Task<LoginResponseModel> Login(LoginRequestModel loginRequest)
        {
            Expression<Func<Account, bool>> searchFilter = x =>
            x.Email.Equals(loginRequest.Email) &&
            x.Password.Equals(PasswordUtil.HashPassword(loginRequest.Password));

            Account account = await _unitOfWork.AccountRepository
                .SingleOrDefaultAsync(predicate: searchFilter, include: x => x.Include(x => x.Role));

            if (account == null) return null;

            RoleEnum role = EnumUtil.ParseEnum<RoleEnum>(account.Role.Name);

            return await BuildLoginResponse(account, role);
        }
        #endregion

        #region Login By Zalo
        public async Task<LoginResponseModel> LoginByZalo(ZaloLoginModel model)
        {
            Account account = await _unitOfWork.AccountRepository
                .SingleOrDefaultAsync(predicate: x => x.ZaloId.Equals(model.ZaloId), include: x => x.Include(x => x.Role));

            if (account != null)
            {
                // 1. If ZaloId exists, return login response
                RoleEnum role = EnumUtil.ParseEnum<RoleEnum>(account.Role.Name);
                return await BuildLoginResponse(account, role);
            }

            // 2. If ZaloId not exists, find account by phone
            if (!string.IsNullOrEmpty(model.Phone))
            {
                account = await _unitOfWork.AccountRepository
                    .SingleOrDefaultAsync(predicate: x => x.Phone.Equals(model.Phone), include: x => x.Include(x => x.Role));

                if (account != null)
                {

                    // If account with phone exists, update ZaloId, image
                    account.ZaloId = model.ZaloId;
                    account.Image_Url = model.Image_Url;
                    await _unitOfWork.AccountRepository.UpdateAsync(account);
                    await _unitOfWork.SaveChangesAsync();

                    RoleEnum role = EnumUtil.ParseEnum<RoleEnum>(account.Role.Name);
                    return await BuildLoginResponse(account, role);
                }
            }
            // 3. If ZaloId and phone not exists return null
            return null;
        }
        #endregion

        #region Logout
        public async Task<ResponseModel> Logout(Guid id)
        {
            try
            {
                var existingRefreshToken = await _unitOfWork.RefreshTokenRepository
                    .SingleOrDefaultAsync(predicate: x => x.AccountId == id && !x.IsRevoked);

                if (existingRefreshToken != null)
                {
                    existingRefreshToken.IsRevoked = true;
                    await _unitOfWork.SaveChangesAsync();
                    _logger.LogInformation($"User with AccountId {id} logged out and revoked refresh token.");

                    return new ResponseModel
                    {
                        IsSuccess = true,
                        Message = "Token has been revoked"
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
                _logger.LogError($"Error revoking token for AccountId {id}: {ex.Message}");
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
                .SingleOrDefaultAsync(predicate: x => x.Id == model.AccountId,
                                      include: x => x.Include(x => x.Role));

            if (account == null)
            {
                return new ResponseModel
                {
                    IsSuccess = false,
                    Message = "Account not found"
                };
            }

            RoleEnum role = EnumUtil.ParseEnum<RoleEnum>(account.Role.Name);
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
            string name = null;
            switch (role)
            {
                case RoleEnum.Student:
                    Guid studentId = await _unitOfWork.StudentRepository
                        .SingleOrDefaultAsync(selector: x => x.Id, predicate: x => x.AccountId.Equals(account.Id));
                    name = await _unitOfWork.StudentRepository
                        .SingleOrDefaultAsync(selector: x => x.Name, predicate: x => x.AccountId.Equals(account.Id));
                    guidClaim = new Tuple<string, Guid>("StudentId", studentId);
                    loginResponseModel = new StudentAccountResponseModel(role,name,studentId);
                    break;

                case RoleEnum.Expert:
                    Guid careerExpertId = await _unitOfWork.CareerExpertRepository
                        .SingleOrDefaultAsync(selector: x => x.Id, predicate: x => x.AccountId.Equals(account.Id));
                    name = await _unitOfWork.CareerExpertRepository
                        .SingleOrDefaultAsync(selector: x => x.Name, predicate: x => x.AccountId.Equals(account.Id));
                    guidClaim = new Tuple<string, Guid>("CareerExpertId", careerExpertId);
                    loginResponseModel = new CareerExpertAccountResponseModel(role,name,careerExpertId);
                    break;

                case RoleEnum.HighSchool:
                    Guid highSchoolId = await _unitOfWork.HighschoolRepository
                        .SingleOrDefaultAsync(selector: x => x.Id, predicate: x => x.AccountId.Equals(account.Id));
                    name = await _unitOfWork.HighschoolRepository
                        .SingleOrDefaultAsync(selector: x => x.Name, predicate: x => x.AccountId.Equals(account.Id));
                    guidClaim = new Tuple<string, Guid>("HighSchoolId", highSchoolId);
                    loginResponseModel = new HighSchoolAccountResponseModel(role, name, highSchoolId);
                    break;

                case RoleEnum.University:
                    Guid universityId = await _unitOfWork.UniversityRepository
                        .SingleOrDefaultAsync(selector: x => x.Id, predicate: x => x.AccountId.Equals(account.Id));
                    name = await _unitOfWork.UniversityRepository
                        .SingleOrDefaultAsync(selector: x => x.Name, predicate: x => x.AccountId.Equals(account.Id));
                    guidClaim = new Tuple<string, Guid>("UniversityId", universityId);
                    loginResponseModel = new UniversityAccountResponseModel(role, name, universityId);
                    break;

                default:
                    loginResponseModel = new LoginResponseModel(role,account.Role.Name);
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




    }
}
