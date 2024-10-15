using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Domain.Entity;
using Domain.Model.ConsultationDay;

namespace Application.Interface.Repository
{
    public interface IConsultationDayRepository  :IGenericRepository<ConsultationDay>
    {
        Task<ConsultationDay?> GetConsultationDayWithTimesByIdAsync(Guid id);

        (Expression<Func<ConsultationDay, bool>> filter, Func<IQueryable<ConsultationDay>, IOrderedQueryable<ConsultationDay>> orderBy) 
            BuildFilterAndOrderBy(ConsultationDaySearchModel searchModel);

    }
}
