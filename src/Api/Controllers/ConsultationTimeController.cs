using Api.Constants;
using Application.Interface.Service;
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

        [HttpPost(ApiEndPointConstant.ConsultationTime.ConsultationTimesEndpoint)]
        public async Task<IActionResult> CreateConsultationTimeAsync([FromForm] ConsultationTimePostModel postModel, Guid consultationDayId)
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
