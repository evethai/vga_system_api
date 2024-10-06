using Api.Constants;
using Api.Validators;
using Application.Interface.Service;
using Domain.Enum;
using Domain.Model.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost(ApiEndPointConstant.Account.LoginEndpoint)]
        public async Task<IActionResult> Login(LoginRequestModel loginRequest)
        {
            if(ModelState.IsValid == false)
            {
                return BadRequest(ModelState);
            }
            var result = await _accountService.Login(loginRequest);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpPost(ApiEndPointConstant.Account.LoginZaloEndpoint)]
        public async Task<IActionResult> LoginZalo(ZaloLoginModel loginZaloRequest)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest(ModelState);
            }
            var result = await _accountService.LoginByZalo(loginZaloRequest);
            if(result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpPost(ApiEndPointConstant.Account.RefreshTokenEndpoint)]
        [Authorize]
        public async Task<IActionResult> RefreshToken(RefreshTokenRequestModel refreshTokenRequest)
        {
            if (ModelState.IsValid == false)
            {
                return BadRequest(ModelState);
            }
            var result = await _accountService.CreateRefreshToken(refreshTokenRequest);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }


        [HttpPost(ApiEndPointConstant.Account.LogoutEndpoint)]
        [Authorize]
        public async Task<IActionResult> Logout(Guid id)
        {
            var result = await _accountService.Logout(id);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

    }
}
