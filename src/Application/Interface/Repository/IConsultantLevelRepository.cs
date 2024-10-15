using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Domain.Entity;
using Domain.Model.ConsultantLevel;

namespace Application.Interface.Repository
{
    public interface IConsultantLevelRepository : IGenericRepository<ConsultantLevel>
    {
        (Expression<Func<ConsultantLevel, bool>> filter, Func<IQueryable<ConsultantLevel>, IOrderedQueryable<ConsultantLevel>> orderBy) 
            BuildFilterAndOrderBy(ConsultantLevelSearchModel searchModel);

    }
}
