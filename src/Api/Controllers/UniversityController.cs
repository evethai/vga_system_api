using Api.Constants;
using Application.Interface.Service;
using Domain.Model.Highschool;
using Domain.Model.University;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UniversityController : ControllerBase
    {
        private readonly IUniversityService _universityService;

        public UniversityController(IUniversityService universityService)
        {
            _universityService = universityService;
        }

        [HttpGet(ApiEndPointConstant.University.UniversitiesEndpoint)]
        public async Task<IActionResult> GetListUniversityAsync([FromQuery]UniversitySearchModel searchModel)
        {
            var result = await _universityService.GetListUniversityAsync(searchModel);
            return Ok(result);
        }
        [HttpGet(ApiEndPointConstant.University.UniversityEndpoint)]
        public async Task<IActionResult> GetUniversityByIdAsync(Guid id)
        {
            var result = await _universityService.GetUniversityByIdAsync(id);
            return Ok(result);
        }
        [HttpPost(ApiEndPointConstant.University.UniversityPostEndpoint)]
        public async Task<IActionResult> CreateUniversityAsync(UniversityPostModel postModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var result = await _universityService.CreateUniversityAsync(postModel);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut(ApiEndPointConstant.University.UniversityPutEndpoint)]
        public async Task<IActionResult> UpdateUniversityAsync(UniversityPutModel putModel, Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {

                var result = await _universityService.UpdateUniversityAsync(putModel, id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpDelete(ApiEndPointConstant.University.UniversityDeleteEndpoint)]
        public async Task<IActionResult> DeleteUniversityAsync(Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var result = await _universityService.DeleteUniversityAsync(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
