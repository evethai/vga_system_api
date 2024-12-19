using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Application.Common.Extensions;
using Application.Interface.Repository;
using Domain.Entity;
using Domain.Model.Major;
using Infrastructure.Data;

namespace Infrastructure.Persistence.Repository
{
    public class MajorRepository : GenericRepository<Major>, IMajorRepository
    {
        private readonly VgaDbContext _context;
        public MajorRepository(VgaDbContext context) : base(context)
        {
            _context = context;
        }

        public (Expression<Func<Major, bool>> filter, Func<IQueryable<Major>, IOrderedQueryable<Major>> orderBy) 
            BuildFilterAndOrderBy(MajorSearchModel searchModel)
        {
            Expression<Func<Major, bool>> filter = p => true;
            Func<IQueryable<Major>, IOrderedQueryable<Major>> orderBy = x => x.OrderBy(x => x.Name);
            if (!string.IsNullOrEmpty(searchModel.name))
            {
                filter = filter.And(m => m.Name.Contains(searchModel.name));
            }
            if (searchModel.majorCategoryId.HasValue)
            {
                filter = filter.And(m => m.MajorCategoryId.Equals(searchModel.majorCategoryId));
            }
            if (searchModel.status.HasValue)
            {
                filter = filter.And(m => m.Status.Equals(searchModel.status));
            }
            return (filter, orderBy);
        }
    }
}
