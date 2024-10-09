using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Application.Common.Extensions;
using Application.Interface.Repository;
using Domain.Entity;
using Domain.Model.University;
using Infrastructure.Data;

namespace Infrastructure.Persistence.Repository
{
    public class UniversityRepository : GenericRepository<University>, IUniversityRepository
    {
        public UniversityRepository(VgaDbContext context) : base(context)
        {
        }

        public (Expression<Func<University, bool>> filter, Func<IQueryable<University>, IOrderedQueryable<University>> orderBy) BuildFilterAndOrderBy(UniversitySearchModel searchModel)
        {
            Expression<Func<University, bool>> filter = p => true;
            Func<IQueryable<University>, IOrderedQueryable<University>> orderBy = null;
            if (!string.IsNullOrEmpty(searchModel.name))
            {
                filter = filter.And(p => p.Name.Contains(searchModel.name));
            }
            if (!string.IsNullOrEmpty(searchModel.Address))
            {
                filter = filter.And(p => p.Name.Contains(searchModel.name));
            }
            return (filter, orderBy);
        }
    }
}
