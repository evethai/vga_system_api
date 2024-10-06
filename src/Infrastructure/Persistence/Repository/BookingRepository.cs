using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Interface.Repository;
using Domain.Entity;
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
    }
}
