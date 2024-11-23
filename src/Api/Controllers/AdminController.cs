using Api.Constants;
using Application.Interface.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        [HttpGet(ApiEndPointConstant.Admin.Dashboard)]
        [Authorize]
        public async Task<IActionResult> GetDashboard()
        {
            var result = await _adminService.GetDashboard();
            return Ok(result);
        }

    }
}
