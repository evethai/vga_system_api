using Api.Constants;
using Api.Validators;
using Application.Interface.Service;
using Domain.Enum;
using Domain.Model.ConsultationTime;
using Domain.Model.TimeSlot;
using Infrastructure.Persistence.Service;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/consultant_times")]
    [ApiController]
    public class ConsultationTimeController : ControllerBase
    {
        private readonly IConsultationTimeService _consultationTimeService;
        public ConsultationTimeController(IConsultationTimeService consultationTimeService)
        {
            _consultationTimeService = consultationTimeService;
        }

        //[CustomAuthorize(RoleEnum.Student, RoleEnum.Consultant)]
        [HttpGet(ApiEndPointConstant.ConsultationTime.ConsultationTimeEndpoint)]
        public async Task<IActionResult> GetConsultationTimeByIdAsync(Guid id)
        {
            var result = await _consultationTimeService.GetConsultationTimeByIdAsync(id);
            return Ok(result);
        }

        //[CustomAuthorize(RoleEnum.Consultant, RoleEnum.Student)]
        [HttpPost(ApiEndPointConstant.ConsultationTime.ConsultationTimesEndpoint)]
        public async Task<IActionResult> CreateConsultationTimeAsync(ConsultationTimePostModel postModel, Guid consultationDayId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var result = await _consultationTimeService.CreateConsultationTimeAsync(postModel, consultationDayId);
                return (result.IsSuccess == false)
                    ? BadRequest(result)
                    : Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //[CustomAuthorize(RoleEnum.Consultant)]
        [HttpDelete(ApiEndPointConstant.ConsultationTime.ConsultationTimeEndpoint)]
        public async Task<IActionResult> DeleteTimeSlotAsync(Guid id)
        {
            try
            {
                var result = await _consultationTimeService.DeleteConsultationTimeAsync(id);
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
