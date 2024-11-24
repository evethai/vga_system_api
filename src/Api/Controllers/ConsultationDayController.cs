using Api.Constants;
using Api.Validators;
using Application.Interface.Service;
using Domain.Enum;
using Domain.Model.ConsultationDay;
using Domain.Model.Student;
using Domain.Model.TimeSlot;
using Infrastructure.Persistence.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    public class ConsultationDayController : ControllerBase
    {
        private readonly IConsultationDayService _consultationDayService;
        public ConsultationDayController(IConsultationDayService consultationDayService)
        {
            _consultationDayService = consultationDayService;
        }

        [Authorize]
        [HttpGet(ApiEndPointConstant.ConsultationDay.ConsultationDaysEndpoint)]
        public async Task<IActionResult> GetListConsultationDaysWithPaginateAsync(ConsultationDaySearchModel searchModel)
        {
            var result = await _consultationDayService.GetListConsultationDaysWithPaginateAsync(searchModel);
            return Ok(result);
        }

        [Authorize]
        [HttpGet(ApiEndPointConstant.ConsultationDay.ConsultationDayEndpoint)]
        public async Task<IActionResult> GetConsultationDayByIdAsync(Guid id)
        {
            var result = await _consultationDayService.GetConsultationDayByIdAsync(id);
            return Ok(result);
        }

        [CustomAuthorize(RoleEnum.Consultant)]
        [HttpPost(ApiEndPointConstant.ConsultationDay.ConsultationDaysEndpoint)]
        public async Task<IActionResult> CreateConsultationDayWithTimesAsync( ConsultationDayPostModel postModel)
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

        [CustomAuthorize(RoleEnum.Consultant)]
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
