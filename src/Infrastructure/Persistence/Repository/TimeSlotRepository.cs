using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Application.Common.Extensions;
using Application.Interface.Repository;
using Domain.Entity;
using Domain.Model.TimeSlot;
using Infrastructure.Data;

namespace Infrastructure.Persistence.Repository
{
    public class TimeSlotRepository : GenericRepository<TimeSlot>, ITimeSlotRepository
    {
        public TimeSlotRepository(VgaDbContext context) : base(context)
        {
        }

        public (Expression<Func<TimeSlot, bool>> filter, Func<IQueryable<TimeSlot>, IOrderedQueryable<TimeSlot>> orderBy)
            BuildFilterAndOrderBy(TimeSlotSearchModel searchModel)
        {
            Expression<Func<TimeSlot, bool>> filter = p => true;
            Func<IQueryable<TimeSlot>, IOrderedQueryable<TimeSlot>> orderBy = null;
            if (!string.IsNullOrEmpty(searchModel.name))
            {
                filter = filter.And(p => p.Name.Contains(searchModel.name));
            }
            if (searchModel.StartTime.HasValue)
            {
                filter = filter.And(p => p.StartTime >= searchModel.StartTime.Value);
            }
            if (searchModel.EndTime.HasValue)
            {
                filter = filter.And(p => p.EndTime <= searchModel.EndTime.Value);
            }
            return (filter, orderBy);
        }
    }
}
