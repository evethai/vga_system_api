using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Application.Common.Extensions;
using Application.Interface.Repository;
using Domain.Entity;
using Domain.Model.Consultant;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repository
{
    public class ConsultantRepository : GenericRepository<Consultant>, IConsultantRepository
    {
        private readonly VgaDbContext _context;
        public ConsultantRepository(VgaDbContext context) : base(context)
        {
            _context = context;
        }

        public (Expression<Func<Consultant, bool>> filter, Func<IQueryable<Consultant>, IOrderedQueryable<Consultant>> orderBy)
            BuildFilterAndOrderBy(ConsultantSearchModel searchModel)
        {
            Expression<Func<Consultant, bool>> filter = p => true;
            Func<IQueryable<Consultant>, IOrderedQueryable<Consultant>> orderBy = null;
            if (!string.IsNullOrEmpty(searchModel.name))
            {
                filter = filter.And(c => c.Account.Name.Contains(searchModel.name));
            }
            if (searchModel.consultantLevelId != 0)
            {
                filter = filter.And(c => c.ConsultantLevelId.Equals(searchModel.consultantLevelId));
            }
            if (searchModel.universityId.HasValue)
            {
                //filter = filter.And(c => c.UniversityId.Equals(searchModel.universityId));
            }
            if (searchModel.status.HasValue)
            {
                filter = filter.And(c => c.Account.Status.Equals((int)searchModel.status.Value));
            }
            return (filter, orderBy);
        }

        public async Task<Consultant?> GetConsultantByIdAsync(Guid id)
        {
            return await _context.Consultant
                .Include(cd => cd.Account)
                    .ThenInclude(a => a.Wallet)
                .FirstOrDefaultAsync(cd => cd.Id.Equals(id));
        }
    }
}

