using Api.Constants;
using Application.Interface.Service;
using Domain.Model.ConsultationDay;
using Domain.Model.TimeSlot;
using Infrastructure.Persistence.Service;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/consultant_days")]
    [ApiController]
    public class ConsultationDayController : ControllerBase
    {
        private readonly IConsultationDayService _consultationDayService;
        public ConsultationDayController(IConsultationDayService consultationDayService)
        {
            _consultationDayService = consultationDayService;
        }

        [HttpGet(ApiEndPointConstant.ConsultationDay.ConsultationDayEndpoint)]
        public async Task<IActionResult> GetConsultationDayByIdAsync(Guid id)
        {
            var result = await _consultationDayService.GetConsultationDayByIdAsync(id);
            return Ok(result);
        }

        [HttpPost(ApiEndPointConstant.ConsultationDay.ConsultationDaysEndpoint)]
        public async Task<IActionResult> CreateConsultationDayWithTimesAsync([FromForm] ConsultationDayPostModel postModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var result = await _consultationDayService.CreateConsultationDayWithTimesAsync(postModel);
                return (result.IsSuccess == false)
                    ? BadRequest(result)
                    : Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete(ApiEndPointConstant.ConsultationDay.ConsultationDayEndpoint)]
        public async Task<IActionResult> DeleteConsultationDayAsync(Guid id)
        {
            try
            {
                var result = await _consultationDayService.DeleteConsultationDayAsync(id);
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
