using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Application.Common.Extensions;
using Application.Interface.Repository;
using Domain.Entity;
using Domain.Model.MajorCategory;
using Infrastructure.Data;

namespace Infrastructure.Persistence.Repository
{
    public class MajorCategoryRepository : GenericRepository<MajorCategory>, IMajorCategoryRepository
    {
        private readonly VgaDbContext _context;
        public MajorCategoryRepository(VgaDbContext context) : base(context)
        {
            _context = context;
        }

        public (Expression<Func<MajorCategory, bool>> filter, Func<IQueryable<MajorCategory>, IOrderedQueryable<MajorCategory>> orderBy) 
            BuildFilterAndOrderBy(MajorCategorySearchModel searchModel)
        {
            Expression<Func<MajorCategory, bool>> filter = p => true;
            Func<IQueryable<MajorCategory>, IOrderedQueryable<MajorCategory>> orderBy = null;
            if (!string.IsNullOrEmpty(searchModel.name))
            {
                filter = filter.And(mc => mc.Name.Contains(searchModel.name));
            }
            if (searchModel.status.HasValue)
            {
                filter = filter.And(mc => mc.Status.Equals(searchModel.status));
            }
            return (filter, orderBy);
        }
    }
}
