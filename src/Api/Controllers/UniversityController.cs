using Api.Constants;
using Api.Validators;
using Application.Interface.Service;
using Domain.Enum;
using Domain.Model.Highschool;
using Domain.Model.University;
using Infrastructure.Persistence.Service;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize]
        [HttpGet(ApiEndPointConstant.University.UniversitiesEndpoint)]
        public async Task<IActionResult> GetListUniversityAsync([FromQuery]UniversitySearchModel searchModel)
        {
            var result = await _universityService.GetListUniversityAsync(searchModel);
            return Ok(result);
        }
        [Authorize]
        [HttpGet(ApiEndPointConstant.University.UniversityEndpoint)]
        public async Task<IActionResult> GetUniversityByIdAsync(Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var result = await _universityService.GetUniversityByIdAsync(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }       
        }
        [CustomAuthorize(RoleEnum.Admin)]
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
        [CustomAuthorize(RoleEnum.Admin, RoleEnum.University)]
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
        [CustomAuthorize(RoleEnum.Admin)]

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
        [CustomAuthorize(RoleEnum.Admin, RoleEnum.University)]
        [HttpPost(ApiEndPointConstant.UniversityLocation.UniversityLocationPostEndpoint)]
        public async Task<IActionResult> CreateUniversityLocationAsync(Guid UniversityId ,List<UniversityLocationModel> postModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var result = await _universityService.CreateUniversityLocationAsync(UniversityId, postModel);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [CustomAuthorize(RoleEnum.Admin, RoleEnum.University)]

        [HttpPut(ApiEndPointConstant.UniversityLocation.UniversityLocationPutEndpoint)]
        public async Task<IActionResult> UpdateUniversityLocationAsync(int id,UniversityLocationPutModel putModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {

                var result = await _universityService.UpdateUniversityLocationAsync(id,putModel);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [CustomAuthorize(RoleEnum.Admin, RoleEnum.University)]

        [HttpDelete(ApiEndPointConstant.UniversityLocation.UniversityLocationDeleteEndpoint)]
        public async Task<IActionResult> DeleteUniversityLocationAsync(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var result = await _universityService.DeleteUniversityLocationAsync(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
