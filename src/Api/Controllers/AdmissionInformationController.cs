using Api.Constants;
using Api.Validators;
using Application.Interface.Service;
using Domain.Enum;
using Domain.Model.AdmissionInformation;
using Domain.Model.University;
using Infrastructure.Persistence.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdmissionInformationController : ControllerBase
    {
        private readonly IAdmissionInformationService _admissionInformationService;
        public AdmissionInformationController(IAdmissionInformationService admissionInformationService)
        {
            _admissionInformationService = admissionInformationService;
        }
        [Authorize]
        [HttpGet(ApiEndPointConstant.AdmisstionInformation.AdmisstionInformationListEndpoint)]
        public async Task<IActionResult> GetListAdmissionInformationAsync([FromQuery] AdmissionInformationSearchModel searchModel)
        {
            var result = await _admissionInformationService.GetListAdmissionInformationAsync(searchModel);
            return Ok(result);
        }
        [Authorize]
        [HttpGet(ApiEndPointConstant.AdmisstionInformation.AdmisstionInformationEndpoint)]
        public async Task<IActionResult> GetAdmissionInformationByIdAsync(int id)
        {
            var result = await _admissionInformationService.GetAdmissionInformationByIdAsync(id);
            return Ok(result);
        }
        [CustomAuthorize(RoleEnum.University)]
        [HttpPost(ApiEndPointConstant.AdmisstionInformation.AdmisstionInformationPostEndpoint)]
        public async Task<IActionResult> CreateAdmissionInformationAsync(Guid UniversityId, List<AdmissionInformationPostModel> postModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var result = await _admissionInformationService.CreateAdmissionInformationAsync(UniversityId, postModel);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut(ApiEndPointConstant.AdmisstionInformation.AdmisstionInformationPutEndpoint)]
        public async Task<IActionResult> UpdateAdmissionInformationAsync(List<AdmissionInformationPutModel> putModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var result = await _admissionInformationService.UpdateAdmissionInformationAsync(putModel);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [CustomAuthorize(RoleEnum.University)]
        [HttpDelete(ApiEndPointConstant.AdmisstionInformation.AdmisstionInformationDeleteEndpoint)]
        public async Task<IActionResult> DeleteAdmissionInformationAsync(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var result = await _admissionInformationService.DeleteAdmissionInformationAsync(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
