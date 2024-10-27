using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Domain.Entity;
using Domain.Model.MajorCategory;

namespace Application.Interface.Repository
{
    public interface IMajorCategoryRepository : IGenericRepository<MajorCategory>
    {
        (Expression<Func<MajorCategory, bool>> filter, Func<IQueryable<MajorCategory>, IOrderedQueryable<MajorCategory>> orderBy)
            BuildFilterAndOrderBy(MajorCategorySearchModel searchModel);
    }
}
