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
    public class CertificationRepository : GenericRepository<Certification>, ICertificationRepository
    {
        private readonly VgaDbContext _context;
        public CertificationRepository(VgaDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task AddRangeAsync(IEnumerable<Certification> certifications)
        {
            await _context.AddRangeAsync(certifications);
        }
        public Task UpdateRangeAsync(IEnumerable<Certification> certifications)
        {
            foreach (var certification in certifications)
            {
                _context.Entry(certification).State = EntityState.Modified;
            }
            return Task.CompletedTask;
        }
    }
}
