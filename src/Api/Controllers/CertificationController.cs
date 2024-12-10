using Api.Constants;
using Api.Validators;
using Application.Interface.Service;
using Domain.Enum;
using Domain.Model.Certification;
using Domain.Model.ConsultationTime;
using Infrastructure.Persistence.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    public class CertificationController  : ControllerBase
    {
        private readonly ICertificationService _certificationService;
        public CertificationController(ICertificationService certificationService)
        {
            _certificationService = certificationService;
        }

        [Authorize]
        [HttpGet(ApiEndPointConstant.Certification.CertificationEndpoint)]
        public async Task<IActionResult> GetCertificationByIdAsync(int id)
        {
            try
            {
                var result = await _certificationService.GetCertificationByIdAsync(id);
                return (result.IsSuccess == false)
                    ? BadRequest(result)
                    : Ok(result);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [CustomAuthorize(RoleEnum.Consultant, RoleEnum.Admin)]
        [HttpPost(ApiEndPointConstant.Certification.CertificationsEndpoint)]
        public async Task<IActionResult> CreateCertificationAsync(CertificationPostModel postModel, Guid consultantId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var result = await _certificationService.CreateCertificationAsync(postModel, consultantId);
                return (result.IsSuccess == false)
                    ? BadRequest(result)
                    : Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [CustomAuthorize(RoleEnum.Consultant, RoleEnum.Admin)]
        [HttpDelete(ApiEndPointConstant.Certification.CertificationEndpoint)]
        public async Task<IActionResult> DeleteCertificationAsync(int id)
        {
            try
            {
                var result = await _certificationService.DeleteCertificationAsync(id);
                return (result.IsSuccess == false)
                    ? BadRequest(result)
                    : Ok(result);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
