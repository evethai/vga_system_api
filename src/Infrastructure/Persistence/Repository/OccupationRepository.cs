using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Application.Common.Extensions;
using Application.Interface.Repository;
using Domain.Entity;
using Domain.Model.Occupation;
using Infrastructure.Data;

namespace Infrastructure.Persistence.Repository
{
    public class OccupationRepository : GenericRepository<Occupation>, IOccupationRepository
    {
        private readonly VgaDbContext _context;
        public OccupationRepository(VgaDbContext context) : base(context)
        {
            _context = context;
        }

        public (Expression<Func<Occupation, bool>> filter, Func<IQueryable<Occupation>, IOrderedQueryable<Occupation>> orderBy) BuildFilterAndOrderBy(OccupationSearchModel searchModel)
        {
            Expression<Func<Occupation, bool>> filter = p => true;
            Func<IQueryable<Occupation>, IOrderedQueryable<Occupation>> orderBy = null;
            if (!string.IsNullOrEmpty(searchModel.name))
            {
                filter = filter.And(o => o.Name.Contains(searchModel.name));
            }
            if (searchModel.entryLevelId.HasValue)
            {
                filter = filter.And(o => o.EntryLevelEducationId.Equals(searchModel.entryLevelId));
            }
            if (searchModel.occupationalGroupId.HasValue)
            {
                filter = filter.And(o => o.OccupationalGroupId.Equals(searchModel.occupationalGroupId));
            }
            if (searchModel.status.HasValue)
            {
                filter = filter.And(o => o.Status.Equals(searchModel.status));
            }
            return (filter, orderBy);
        }
    }
}
