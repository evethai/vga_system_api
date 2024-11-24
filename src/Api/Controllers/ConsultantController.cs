using Api.Constants;
using Api.Validators;
using Application.Interface.Service;
using Domain.Enum;
using Domain.Model.Consultant;
using Domain.Model.Student;
using Infrastructure.Persistence.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    public class ConsultantController : ControllerBase
    {
        private readonly IConsultantService _consultantService;
        public ConsultantController(IConsultantService consultantService)
        {
            _consultantService = consultantService;
        }

        [Authorize]
        [HttpGet(ApiEndPointConstant.Consultant.ConsultantsEndpoint)]
        public async Task<IActionResult> GetListConsultantsWithPaginateAsync(ConsultantSearchModel searchModel)
        {
            try
            {
                var result = await _consultantService.GetListConsultantsWithPaginateAsync(searchModel);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpGet(ApiEndPointConstant.Consultant.ConsultantEndpoint)]
        public async Task<IActionResult> GetConsultantByIdAsync(Guid id)
        {
            try
            {
                var result = await _consultantService.GetConsultantByIdAsync(id);
                return (result.IsSuccess == false)
                    ? BadRequest(result)
                    : Ok(result);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [CustomAuthorize(RoleEnum.University)]
        [HttpPost(ApiEndPointConstant.Consultant.ConsultantsEndpoint)]
        public async Task<IActionResult> CreateConsultantAsyns(ConsultantPostModel postModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var result = await _consultantService.CreateConsultantAsync(postModel);
                return (result.IsSuccess == false)
                    ? BadRequest(result)
                    : Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [CustomAuthorize(RoleEnum.University)]
        [HttpPut(ApiEndPointConstant.Consultant.ConsultantEndpoint)]
        public async Task<IActionResult> UpdateConsultantAsync(ConsultantPutModel putModel, Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var result = await _consultantService.UpdateConsultantAsync(id, putModel);
                return (result.IsSuccess == false)
                    ? BadRequest(result)
                    : Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [CustomAuthorize(RoleEnum.University,RoleEnum.Admin)]
        [HttpDelete(ApiEndPointConstant.Consultant.ConsultantEndpoint)]
        public async Task<IActionResult> DeleteConsultantAsync(Guid id)
        {
            try
            {
                var result = await _consultantService.DeleteConsultantAsync(id);
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
