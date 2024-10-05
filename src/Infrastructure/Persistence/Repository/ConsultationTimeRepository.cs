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
    public class ConsultationTimeRepository : GenericRepository<ConsultationTime>, IConsultationTimeRepository
    {
        private readonly VgaDbContext _context;
        public ConsultationTimeRepository(VgaDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task AddRangeAsync(IEnumerable<ConsultationTime> consultationTimes)
        {
            await _context.AddRangeAsync(consultationTimes);
        }
    }
}
