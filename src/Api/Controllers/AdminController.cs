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
        public async Task<IActionResult> GetDashboard()
        {
            var result = await _adminService.GetDashboard();
            return Ok(result);
        }

        [HttpGet(ApiEndPointConstant.Admin.UniversityDashboard)]
        public async Task<IActionResult> GetUniDashboard(Guid id)
        {
            var result = await _adminService.GetUniversityDashBoard(id);
            return Ok(result);
        }

    }
}
