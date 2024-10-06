using Application.Interface.Service;
using Domain.Model.Account;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("login")]
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

        [HttpPost("login-zalo")]
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

    }
}
