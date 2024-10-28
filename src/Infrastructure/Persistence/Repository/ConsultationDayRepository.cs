using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Extensions;
using Application.Interface.Repository;
using Domain.Entity;
using Domain.Model.ConsultationDay;
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
            return await _context.ConsultationDay
                .Include(cd => cd.ConsultationTimes)
                .FirstOrDefaultAsync(cd => cd.Id == id);
        }

        public (Expression<Func<ConsultationDay, bool>> filter, Func<IQueryable<ConsultationDay>, IOrderedQueryable<ConsultationDay>> orderBy) 
            BuildFilterAndOrderBy(ConsultationDaySearchModel searchModel)
        {
            Expression<Func<ConsultationDay, bool>> filter = p => true;
            Func<IQueryable<ConsultationDay>, IOrderedQueryable<ConsultationDay>> orderBy = null;

            if (!string.IsNullOrEmpty(searchModel.name))
            {
                filter = filter.And(p => p.Consultant.Account.Name.Contains(searchModel.name));
            }
            if (searchModel.Day.HasValue)
            {
                filter = filter.And(p => p.Day.Equals(searchModel.Day.Value));
            }
            if (searchModel.ConsultantId.HasValue)
            {
                filter = filter.And(p => p.ConsultantId.Equals(searchModel.ConsultantId.Value));
            }
            return (filter, orderBy);
        }

    }
}
