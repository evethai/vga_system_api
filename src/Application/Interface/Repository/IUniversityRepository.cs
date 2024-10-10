using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Domain.Entity;
using Domain.Model.Highschool;
using Domain.Model.University;

namespace Application.Interface.Repository
{
    public interface IUniversityRepository : IGenericRepository<University>
    {
        (Expression<Func<University, bool>> filter, Func<IQueryable<University>, IOrderedQueryable<University>> orderBy) BuildFilterAndOrderBy(UniversitySearchModel searchModel);

    }
}
