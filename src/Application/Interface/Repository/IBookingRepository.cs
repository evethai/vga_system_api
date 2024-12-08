using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Domain.Entity;
using Domain.Model.Booking;
using Domain.Model.Consultant;
using Domain.Model.Response;

namespace Application.Interface.Repository
{
    public interface IBookingRepository : IGenericRepository<Booking>
    {
        Task<List<Booking>> GetAllBookingsWithDetailsAsync();

        (Expression<Func<Booking, bool>> filter, Func<IQueryable<Booking>, IOrderedQueryable<Booking>> orderBy)
            BuildFilterAndOrderBy(BookingSearchModel searchModel);

        Task SaveBookingDataAsync(
            ConsultationTime consultationTime,
            Wallet studentWallet,
            Wallet consultantWallet,
            Booking booking,
            Transaction studentTransaction);

        Task<ResponseModel> ProcessBooking(Guid bookingId, BookingConsultantUpdateModel model);
        Task<ResponseModel> ProcessReport(Guid bookingId, BookingProcessReportModel model);
    }
}
