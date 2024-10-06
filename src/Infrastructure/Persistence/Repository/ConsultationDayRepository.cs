using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Interface.Repository;
using Domain.Entity;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repository
{
    public class ConsultationDayRepository : GenericRepository<ConsultationDay>, IConsultationDayRepository
    {
        private readonly VgaDbContext _context;
        public ConsultationDayRepository(VgaDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<ConsultationDay?> GetConsultationDayWithTimesByIdAsync(Guid id)
        {
            return await _context.consultation_day
                .Include(cd => cd.ConsultationTimes)
                .FirstOrDefaultAsync(cd => cd.Id == id);
        }
    }
}
