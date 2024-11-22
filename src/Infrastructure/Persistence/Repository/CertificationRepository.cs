using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Interface.Repository;
using Domain.Entity;
using Infrastructure.Data;

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
    }
}
