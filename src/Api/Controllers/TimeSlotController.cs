using Api.Constants;
using Api.Validators;
using Application.Interface.Service;
using Domain.Enum;
using Domain.Model.Student;
using Domain.Model.TimeSlot;
using Infrastructure.Persistence.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/time_slots")]
    [ApiController]
    public class TimeSlotController : ControllerBase
    {
        private readonly ITimeSlotService _timeSlotService;
        public TimeSlotController(ITimeSlotService timeSlotService)
        {
            _timeSlotService = timeSlotService;
        }

        [Authorize]
        [HttpGet(ApiEndPointConstant.TimeSlot.TimeSlotsEndpoint)]
        public async Task<IActionResult> GetListTimeSlotsWithPaginateAsync(TimeSlotSearchModel searchModel)
        {
            var result = await _timeSlotService.GetListTimeSlotsWithPaginateAsync(searchModel);
            return Ok(result);
        }

        [Authorize]
        [HttpGet(ApiEndPointConstant.TimeSlot.TimeSlotEndpoint)]
        public async Task<IActionResult> GetTimeSlotByIdAsync(int id)
        {
            try
            {
                var result = await _timeSlotService.GetTimeSlotByIdAsync(id);
                return (result.IsSuccess == false)
                    ? BadRequest(result)
                    : Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [CustomAuthorize(RoleEnum.Admin)]
        [HttpPost(ApiEndPointConstant.TimeSlot.TimeSlotsEndpoint)]
        public async Task<IActionResult> CreateTimeSlotAsync(TimeSlotPostModel postModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var result = await _timeSlotService.CreateTimeSlotAsync(postModel);
                return (result.IsSuccess == false)
                    ? BadRequest(result)
                    : Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [CustomAuthorize(RoleEnum.Admin)]
        [HttpPut(ApiEndPointConstant.TimeSlot.TimeSlotsEndpoint)]
        public async Task<IActionResult> UpdateTimeSlotAsync(TimeSlotPutModel putModel, int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var result = await _timeSlotService.UpdateTimeSlotAsync(putModel, id);
                return (result.IsSuccess == false)
                    ? BadRequest(result)
                    : Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [CustomAuthorize(RoleEnum.Admin)]
        [HttpDelete(ApiEndPointConstant.TimeSlot.TimeSlotsEndpoint)]
        public async Task<IActionResult> DeleteTimeSlotAsync(int id)
        {
            try
            {
                var result = await _timeSlotService.DeleteTimeSlotAsync(id);
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
