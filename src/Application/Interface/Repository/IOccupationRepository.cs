using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Domain.Entity;
using Domain.Model.Occupation;

namespace Application.Interface.Repository
{
    public interface IOccupationRepository : IGenericRepository<Occupation>
    {
        (Expression<Func<Occupation, bool>> filter, Func<IQueryable<Occupation>, IOrderedQueryable<Occupation>> orderBy)
            BuildFilterAndOrderBy(OccupationSearchModel searchModel);
    }
}
