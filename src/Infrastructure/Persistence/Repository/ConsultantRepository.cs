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

namespace Infrastructure.Persistence.Repository
{
    public class ConsultantRepository : GenericRepository<Consultant>, IConsultantRepository
    {
        public ConsultantRepository(VgaDbContext context) : base(context)
        {
        }

        public (Expression<Func<Consultant, bool>> filter, Func<IQueryable<Consultant>, IOrderedQueryable<Consultant>> orderBy) BuildFilterAndOrderBy(ConsultantSearchModel searchModel)
        {
            Expression<Func<Consultant, bool>> filter = p => true;
            Func<IQueryable<Consultant>, IOrderedQueryable<Consultant>> orderBy = null;
            if (!string.IsNullOrEmpty(searchModel.name))
            {
                filter = filter.And(p => p.Name.Contains(searchModel.name));
            }
            return (filter, orderBy);
        }
    }
}

