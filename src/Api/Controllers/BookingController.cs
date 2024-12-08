using Api.Constants;
using Api.Validators;
using Application.Interface.Service;
using Domain.Enum;
using Domain.Model.Booking;
using Domain.Model.Consultant;
using Domain.Model.TimeSlot;
using Infrastructure.Persistence.Service;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly IBookingService _bookingService;
        public BookingController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        [CustomAuthorize(RoleEnum.Consultant, RoleEnum.Student)]
        [HttpGet(ApiEndPointConstant.Booking.BookingsEndpoint)]
        public async Task<IActionResult> GetListBookingsWithPaginateAsync(BookingSearchModel searchModel)
        {
            try
            {
                var result = await _bookingService.GetListBookingsWithPaginateAsync(searchModel);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [CustomAuthorize(RoleEnum.Consultant, RoleEnum.Student)]
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

        [CustomAuthorize(RoleEnum.Student)]
        [HttpPost(ApiEndPointConstant.Booking.BookingsEndpoint)]
        public async Task<IActionResult> BookConsultationTimeAsync(Guid consultationTimeId, Guid studentId)
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

        [CustomAuthorize(RoleEnum.Consultant)]
        [HttpPut(ApiEndPointConstant.Booking.BookingEndpoint)]
        public async Task<IActionResult> ProcessBookingAsync(Guid id, BookingConsultantUpdateModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var result = await _bookingService.ProcessBookingAsync(id, model);
                return (result.IsSuccess == false)
                    ? BadRequest(result)
                    : Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [CustomAuthorize(RoleEnum.Student)]
        [HttpPut(ApiEndPointConstant.Booking.ReportBookingEndpoint)]
        public async Task<IActionResult> ReportBookingAsync(Guid id, BookingStudentReportModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var result = await _bookingService.ReportBookingAsync(id, model);
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
        [HttpPut(ApiEndPointConstant.Booking.ProcessReportBookingEndpoint)]
        public async Task<IActionResult> ProcessReportBookingAsync(Guid id, BookingProcessReportModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var result = await _bookingService.ProcessReportBookingAsync(id, model);
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
