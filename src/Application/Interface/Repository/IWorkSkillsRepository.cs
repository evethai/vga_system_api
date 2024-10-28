using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Domain.Entity;
using Domain.Model.WorkSkills;

namespace Application.Interface.Repository
{
    public interface IWorkSkillsRepository : IGenericRepository<WorkSkills>
    {
        (Expression<Func<WorkSkills, bool>> filter, Func<IQueryable<WorkSkills>, IOrderedQueryable<WorkSkills>> orderBy)
            BuildFilterAndOrderBy(WorkSkillsSearchModel searchModel);
    }
}
