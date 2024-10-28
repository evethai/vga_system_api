using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Domain.Entity;
using Domain.Model.OccupationalGroup;

namespace Application.Interface.Repository
{
    public interface IOccupationalGroupRepository : IGenericRepository<OccupationalGroup>
    {
        (Expression<Func<OccupationalGroup, bool>> filter, Func<IQueryable<OccupationalGroup>, IOrderedQueryable<OccupationalGroup>> orderBy)
            BuildFilterAndOrderBy(OccupationalGroupSearchModel searchModel);
    }
}
