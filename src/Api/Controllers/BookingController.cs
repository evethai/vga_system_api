using Api.Constants;
using Application.Interface.Service;
using Domain.Model.TimeSlot;
using Infrastructure.Persistence.Service;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/consultants")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly IBookingService _bookingService;
        public BookingController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        [HttpGet(ApiEndPointConstant.Booking.BookingEndpoint)]
        public async Task<IActionResult> GetBookingByIdAsync(Guid id)
        {
            try
            {
                var result = await _bookingService.GetBookingByIdAsync(id);
                return (result.IsSuccess == false)
                    ? BadRequest(result)
                    : Ok(result);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet(ApiEndPointConstant.Booking.BookingsEndpoint)]
        public async Task<IActionResult> GetAllBookingsAsync()
        {
            try
            {
                var result = await _bookingService.GetAllBookingsAsync();
                return (result.IsSuccess == false)
                    ? BadRequest(result)
                    : Ok(result);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost(ApiEndPointConstant.Booking.BookingsEndpoint)]
        public async Task<IActionResult> BookConsultationTimeAsync([FromForm] Guid consultationTimeId, Guid studentId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var result = await _bookingService.BookConsultationTimeAsync(consultationTimeId, studentId);
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
