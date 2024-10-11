using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Domain.Entity;
using Domain.Model.TimeSlot;

namespace Application.Interface.Repository
{
    public interface ITimeSlotRepository : IGenericRepository<TimeSlot>
    {
        (Expression<Func<TimeSlot, bool>> filter, Func<IQueryable<TimeSlot>, IOrderedQueryable<TimeSlot>> orderBy) 
            BuildFilterAndOrderBy(TimeSlotSearchModel searchModel);

    }
}
