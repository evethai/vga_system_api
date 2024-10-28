using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Application.Common.Extensions;
using Application.Interface.Repository;
using Domain.Entity;
using Domain.Model.EntryLevelEducation;
using Infrastructure.Data;

namespace Infrastructure.Persistence.Repository
{
    public class EntryLevelEducationRepository : GenericRepository<EntryLevelEducation>, IEntryLevelEducationRepository
    {
        private readonly VgaDbContext _context;
        public EntryLevelEducationRepository(VgaDbContext context) : base(context)
        {
            _context = context;
        }

        public (Expression<Func<EntryLevelEducation, bool>> filter, Func<IQueryable<EntryLevelEducation>, IOrderedQueryable<EntryLevelEducation>> orderBy) 
            BuildFilterAndOrderBy(EntryLevelEducationSearchModel searchModel)
        {
            Expression<Func<EntryLevelEducation, bool>> filter = p => true;
            Func<IQueryable<EntryLevelEducation>, IOrderedQueryable<EntryLevelEducation>> orderBy = null;
            if (!string.IsNullOrEmpty(searchModel.name))
            {
                filter = filter.And(e => e.Name.Contains(searchModel.name));
            }
            if (searchModel.status.HasValue)
            {
                filter = filter.And(e => e.Status.Equals(searchModel.status));
            }
            return (filter, orderBy);
        }
    }
}
