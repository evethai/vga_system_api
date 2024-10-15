using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Application.Common.Extensions;
using Application.Interface.Repository;
using Domain.Entity;
using Domain.Model.ConsultantLevel;
using Infrastructure.Data;

namespace Infrastructure.Persistence.Repository
{
    public class ConsultantLevelRepository : GenericRepository<ConsultantLevel>, IConsultantLevelRepository
    {
        public ConsultantLevelRepository(VgaDbContext context) : base(context)
        {
        }

        public (Expression<Func<ConsultantLevel, bool>> filter, Func<IQueryable<ConsultantLevel>, IOrderedQueryable<ConsultantLevel>> orderBy) BuildFilterAndOrderBy(ConsultantLevelSearchModel searchModel)
        {
            Expression<Func<ConsultantLevel, bool>> filter = p => true;
            Func<IQueryable<ConsultantLevel>, IOrderedQueryable<ConsultantLevel>> orderBy = null;
            if (!string.IsNullOrEmpty(searchModel.name))
            {
                filter = filter.And(p => p.Name.Contains(searchModel.name));
            }
            return (filter, orderBy);
        }
    }
}
