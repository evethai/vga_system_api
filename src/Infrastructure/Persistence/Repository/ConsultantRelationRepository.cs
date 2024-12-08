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
    public class ConsultantRelationRepository : GenericRepository<ConsultantRelation>, IConsultantRelationRepository
    {
        private readonly VgaDbContext _context;
        public ConsultantRelationRepository(VgaDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task AddRangeAsync(IEnumerable<ConsultantRelation> consultantRelations)
        {
            await _context.AddRangeAsync(consultantRelations);
        }

        public void DeleteRange(IEnumerable<ConsultantRelation> relations)
        {
            _context.RemoveRange(relations);
        }
    }
}
