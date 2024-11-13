using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Domain.Entity;
using Domain.Model.EntryLevelEducation;

namespace Application.Interface.Repository
{
    public interface IEntryLevelEducationRepository : IGenericRepository<EntryLevelEducation>
    {
        (Expression<Func<EntryLevelEducation, bool>> filter, Func<IQueryable<EntryLevelEducation>, IOrderedQueryable<EntryLevelEducation>> orderBy)
            BuildFilterAndOrderBy(EntryLevelEducationSearchModel searchModel);

    }
}
