using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Model.Booking;
using Domain.Model.Response;

namespace Application.Interface.Service
{
    public interface IBookingService
    {
        Task<ResponseModel> BookConsultationTimeAsync(Guid consultationTimeId, Guid studentId);
        Task<ResponseModel> GetBookingByIdAsync(Guid bookingId);
        Task<ResponseModel> GetAllBookingsAsync();
        Task<ResponseBookingModel> GetListBookingsWithPaginateAsync(BookingSearchModel searchModel);
        Task<ResponseModel> ProcessBookingAsync(Guid bookingId, BookingConsultantUpdateModel model);
        Task<ResponseModel> ReportBookingAsync(Guid bookingId, BookingStudentReportModel model);
        Task<ResponseModel> ProcessReportBookingAsync(Guid bookingId, BookingProcessReportModel model);

    }
}
