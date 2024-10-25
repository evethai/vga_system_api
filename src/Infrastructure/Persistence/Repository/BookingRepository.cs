using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Application.Common.Extensions;
using Application.Interface.Repository;
using Domain.Entity;
using Domain.Model.Booking;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repository
{
    public class BookingRepository : GenericRepository<Booking>, IBookingRepository
    {
        private readonly VgaDbContext _context;
        public BookingRepository(VgaDbContext context) : base(context)
        {
            _context = context;
        }

        public (Expression<Func<Booking, bool>> filter, Func<IQueryable<Booking>, IOrderedQueryable<Booking>> orderBy) 
            BuildFilterAndOrderBy(BookingSearchModel searchModel)
        {
            Expression<Func<Booking, bool>> filter = p => true;
            Func<IQueryable<Booking>, IOrderedQueryable<Booking>> orderBy = null;
            if (!string.IsNullOrEmpty(searchModel.consultantName))
            {
                filter = filter.And(p => p.ConsultationTime.Day.Consultant.Account.Name.Contains(searchModel.consultantName));
            }
            if (!string.IsNullOrEmpty(searchModel.studentName))
            {
                filter = filter.And(p => p.Student.Account.Name.Contains(searchModel.studentName));
            }
            if (searchModel.studentId.HasValue)
            {
                filter = filter.And(p => p.StudentId.Equals(searchModel.studentId.Value));
            }
            if (searchModel.consultantId.HasValue)
            {
                filter = filter.And(p => p.ConsultationTime.Day.ConsultantId.Equals(searchModel.consultantId.Value));
            }
            return (filter, orderBy);
        }

        public async Task<List<Booking>> GetAllBookingsWithDetailsAsync()
        {
            return await _context.Booking
                .Include(b => b.ConsultationTime)
                    .ThenInclude(ct => ct.SlotTime)
                .Include(b => b.ConsultationTime.Day)
                    .ThenInclude(cd => cd.Consultant)
                        .ThenInclude(e => e.Account)
                .Include(b => b.Student)
                    .ThenInclude(s => s.Account)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task SaveBookingDataAsync(
            ConsultationTime consultationTime,
            Wallet studentWallet,
            Wallet consultantWallet,
            Booking booking,
            Transaction studentTransaction,
            Transaction consultantTransaction)
        {
            // Update consultation time
            _context.ConsultationTime.Update(consultationTime);

            // Update student and consultant wallets
            _context.Wallet.Update(studentWallet);
            _context.Wallet.Update(consultantWallet);

            // Add booking and transactions
            await _context.Booking.AddAsync(booking);
            await _context.Transaction.AddRangeAsync(studentTransaction, consultantTransaction);

            // Save all changes
            await _context.SaveChangesAsync();
        }

    }
}
