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
    public class OccupationSkillRepository : GenericRepository<OccupationalSKills>, IOccupationSkillRepository
    {
        private readonly VgaDbContext _context;
        public OccupationSkillRepository(VgaDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task AddRangeAsync(IEnumerable<OccupationalSKills> occupationalSkills)
        {
            await _context.AddRangeAsync(occupationalSkills);
        }
    }
}
