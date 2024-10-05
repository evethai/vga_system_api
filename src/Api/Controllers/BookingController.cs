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
            var result = await _bookingService.GetBookingByIdAsync(id);
            return Ok(result);
        }

        [HttpGet(ApiEndPointConstant.Booking.BookingsEndpoint)]
        public async Task<IActionResult> GetAllBookingsAsync()
        {
            var result = await _bookingService.GetAllBookingsAsync();
            return Ok(result);
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
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
