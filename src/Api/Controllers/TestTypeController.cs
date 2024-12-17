using Api.Constants;
using Api.Validators;
using Application.Interface.Service;
using Domain.Enum;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [CustomAuthorize(RoleEnum.Admin, RoleEnum.Student)]
    public class TestTypeController : ControllerBase
    {
        private readonly ITestTypeService _testTypeService;
        public TestTypeController(ITestTypeService testTypeService)
        {
            _testTypeService = testTypeService;
        }
        [HttpGet(ApiEndPointConstant.TestType.TestTypesEndpoint)]
        public async Task<IActionResult> GetAllTestTypes()
        {
            var testTypes = await _testTypeService.GetAllTestTypes();
            return Ok(testTypes);
        }

        [HttpGet(ApiEndPointConstant.TestType.TestTypeEndpoint)]
        public async Task<IActionResult> GetTestTypeById(Guid id)
        {
            var testType = await _testTypeService.GetTestTypeById(id);
            return Ok(testType);
        }

        [HttpPut(ApiEndPointConstant.TestType.UpdateTestTypeEndpoint)]
        public async Task<IActionResult> UpdatePointTestType (Guid id, int point)
        {
            var result = await _testTypeService.UpdateTestTypeById(id, point);
            return Ok(result);
        }
    }
}
