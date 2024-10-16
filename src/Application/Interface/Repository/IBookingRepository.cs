using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Domain.Entity;
using Domain.Model.Booking;
using Domain.Model.Consultant;

namespace Application.Interface.Repository
{
    public interface IBookingRepository : IGenericRepository<Booking>
    {
        Task<List<Booking>> GetAllBookingsWithDetailsAsync();

        (Expression<Func<Booking, bool>> filter, Func<IQueryable<Booking>, IOrderedQueryable<Booking>> orderBy)
            BuildFilterAndOrderBy(BookingSearchModel searchModel);

    }
}
