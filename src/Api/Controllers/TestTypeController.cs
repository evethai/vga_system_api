using Api.Constants;
using Application.Interface.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
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
    }
}
