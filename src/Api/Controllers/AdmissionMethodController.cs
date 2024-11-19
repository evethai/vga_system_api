using Api.Constants;
using Api.Validators;
using Application.Interface.Service;
using Domain.Enum;
using Domain.Model.AdmissionInformation;
using Infrastructure.Persistence.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdmissionMethodController : ControllerBase
    {
        private readonly IAdmissionMethodService _admissionMethodService;

        public AdmissionMethodController(IAdmissionMethodService admissionMethodService)
        {
            _admissionMethodService = admissionMethodService;
        }
        [Authorize]
        [HttpGet(ApiEndPointConstant.AdmisstionMethod.AdmisstionMethodListEndpoint)]
        public async Task<IActionResult> GetListAdmissionMethodAsync([FromQuery] AdmissionMethodSearchModel searchModel)
        {
            var result = await _admissionMethodService.GetListAdmissionMethodAsync(searchModel);
            return Ok(result);
        }
        [Authorize]
        [HttpGet(ApiEndPointConstant.AdmisstionMethod.AdmisstionMethodEndpoint)]
        public async Task<IActionResult> GetAdmissionMethodByIdAsync(Guid id)
        {
            var result = await _admissionMethodService.GetAdmissionMethodById(id);
            return Ok(result);
        }
        [CustomAuthorize(RoleEnum.Admin)]
        [HttpPost(ApiEndPointConstant.AdmisstionMethod.AdmisstionMethodPostEndpoint)]
        public async Task<IActionResult> CreateAdmissionMethodAsync(AdmissionMethodPostModel postModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var result = await _admissionMethodService.CreateAdmissionMethodAsync(postModel);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [CustomAuthorize(RoleEnum.Admin)]
        [HttpPut(ApiEndPointConstant.AdmisstionMethod.AdmisstionMethodPutEndpoint)]
        public async Task<IActionResult> UpdateAdmissionMethodAsync(Guid id, AdmissionMethodPutModel putModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var result = await _admissionMethodService.UpdateAdmissionMethodAsync(id, putModel);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [CustomAuthorize(RoleEnum.Admin)]
        [HttpDelete(ApiEndPointConstant.AdmisstionMethod.AdmisstionMethodDeleteEndpoint)]
        public async Task<IActionResult> DeleteAdmissionMethodAsync(Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var result = await _admissionMethodService.DeleteAdmissionMethodAsync(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
