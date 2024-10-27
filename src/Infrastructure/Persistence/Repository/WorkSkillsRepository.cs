using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Application.Common.Extensions;
using Application.Interface.Repository;
using Domain.Entity;
using Domain.Model.WorkSkills;
using Infrastructure.Data;

namespace Infrastructure.Persistence.Repository
{
    public class WorkSkillsRepository : GenericRepository<WorkSkills>, IWorkSkillsRepository
    {
        private readonly VgaDbContext _context;
        public WorkSkillsRepository(VgaDbContext context) : base(context)
        {
            _context = context;
        }

        public (Expression<Func<WorkSkills, bool>> filter, Func<IQueryable<WorkSkills>, IOrderedQueryable<WorkSkills>> orderBy) BuildFilterAndOrderBy(WorkSkillsSearchModel searchModel)
        {
            Expression<Func<WorkSkills, bool>> filter = p => true;
            Func<IQueryable<WorkSkills>, IOrderedQueryable<WorkSkills>> orderBy = null;
            if (!string.IsNullOrEmpty(searchModel.name))
            {
                filter = filter.And(ws => ws.Name.Contains(searchModel.name));
            }
            if (searchModel.status.HasValue)
            {
                filter = filter.And(ws => ws.Status.Equals(searchModel.status));
            }
            return (filter, orderBy);
        }
    }
}
