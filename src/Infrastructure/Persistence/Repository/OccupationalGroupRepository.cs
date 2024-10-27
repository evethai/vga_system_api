using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Application.Common.Extensions;
using Application.Interface.Repository;
using Domain.Entity;
using Domain.Model.OccupationalGroup;
using Infrastructure.Data;

namespace Infrastructure.Persistence.Repository
{
    public class OccupationalGroupRepository : GenericRepository<OccupationalGroup>, IOccupationalGroupRepository
    {
        private readonly VgaDbContext _context;
        public OccupationalGroupRepository(VgaDbContext context) : base(context)
        {
            _context = context;
        }

        public (Expression<Func<OccupationalGroup, bool>> filter, Func<IQueryable<OccupationalGroup>, IOrderedQueryable<OccupationalGroup>> orderBy) BuildFilterAndOrderBy(OccupationalGroupSearchModel searchModel)
        {
            Expression<Func<OccupationalGroup, bool>> filter = p => true;
            Func<IQueryable<OccupationalGroup>, IOrderedQueryable<OccupationalGroup>> orderBy = null;
            if (!string.IsNullOrEmpty(searchModel.name))
            {
                filter = filter.And(og => og.Name.Contains(searchModel.name));
            }
            if (searchModel.status.HasValue)
            {
                filter = filter.And(og => og.Status.Equals(searchModel.status));
            }
            return (filter, orderBy);
        }
    }
}
