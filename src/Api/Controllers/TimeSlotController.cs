﻿using Api.Constants;
using Application.Interface.Service;
using Domain.Model.Student;
using Domain.Model.TimeSlot;
using Infrastructure.Persistence.Service;
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

        [HttpGet(ApiEndPointConstant.TimeSlot.TimeSlotEndpoint)]
        public async Task<IActionResult> GetTimeSlotByIdAsync(int id)
        {
            var result = await _timeSlotService.GetTimeSlotByIdAsync(id);
            return Ok(result);
        }

        [HttpPost(ApiEndPointConstant.TimeSlot.TimeSlotsEndpoint)]
        public async Task<IActionResult> CreateTimeSlotAsync([FromForm] TimeSlotPostModel postModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var result = await _timeSlotService.CreateTimeSlotAsync(postModel);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut(ApiEndPointConstant.TimeSlot.TimeSlotsEndpoint)]
        public async Task<IActionResult> UpdateTimeSlotAsync([FromForm] TimeSlotPutModel putModel, int timeSlotId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var result = await _timeSlotService.UpdateTimeSlotAsync(putModel, timeSlotId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete(ApiEndPointConstant.TimeSlot.TimeSlotsEndpoint)]
        public async Task<IActionResult> DeleteTimeSlotAsync(int id)
        {
            var result = await _timeSlotService.DeleteTimeSlotAsync(id);
            return Ok(result);
        }

        [HttpGet(ApiEndPointConstant.TimeSlot.TimeSlotsEndpoint)]
        public async Task<IActionResult> GetAllTimeSlotsAsync()
        {
            var result = await _timeSlotService.GetAllTimeSlotsAsync();
            return Ok(result);
        }

    }
}
