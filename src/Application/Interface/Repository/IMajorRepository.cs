using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Domain.Entity;
using Domain.Model.Major;

namespace Application.Interface.Repository
{
    public interface IMajorRepository : IGenericRepository<Major>
    {
        (Expression<Func<Major, bool>> filter, Func<IQueryable<Major>, IOrderedQueryable<Major>> orderBy)
            BuildFilterAndOrderBy(MajorSearchModel searchModel);
    }
}
