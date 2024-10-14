using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Domain.Entity;
using Domain.Model.Consultant;
using Domain.Model.Student;

namespace Application.Interface.Repository
{
    public interface IConsultantRepository : IGenericRepository<Consultant>
    {
        (Expression<Func<Consultant, bool>> filter, Func<IQueryable<Consultant>, IOrderedQueryable<Consultant>> orderBy) 
            BuildFilterAndOrderBy(ConsultantSearchModel searchModel);

        Task<Consultant?> GetConsultantByIdAsync(Guid id);
    }
}
