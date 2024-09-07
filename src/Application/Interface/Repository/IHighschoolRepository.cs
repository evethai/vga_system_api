using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Domain.Entity;
using Domain.Model.Highschool;

namespace Application.Interface.Repository;
public interface IHighschoolRepository: IGenericRepository<HighSchool>
{
    (Expression<Func<HighSchool, bool>> filter, Func<IQueryable<HighSchool>, IOrderedQueryable<HighSchool>> orderBy) BuildFilterAndOrderBy(HighschoolSearchModel searchModel);
}
