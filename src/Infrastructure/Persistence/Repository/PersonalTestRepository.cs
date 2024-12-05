using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Application.Common.Extensions;
using Application.Interface.Repository;
using Domain.Entity;
using Domain.Model.PersonalTest;
using Domain.Model.Question;
using Infrastructure.Data;

namespace Infrastructure.Persistence.Repository
{
    public class PersonalTestRepository : GenericRepository<PersonalTest>, IPersonalTestRepository
    {
        public PersonalTestRepository(VgaDbContext context) : base(context)
        {
        }
        public (Expression<Func<PersonalTest, bool>> filter, Func<IQueryable<PersonalTest>, IOrderedQueryable<PersonalTest>> orderBy)
        BuildFilterAndOrderBy(PersonalTestSearchModel searchModel)
        {
            Expression<Func<PersonalTest, bool>> filter = p => p.Status == true;
            Func<IQueryable<PersonalTest>, IOrderedQueryable<PersonalTest>> orderBy = null;
            if (!string.IsNullOrEmpty(searchModel.Name))
            {
                filter = filter.And(c => c.Name.Contains(searchModel.Name) || c.Description.Contains(searchModel.Name));
            }
            orderBy = q => q.OrderByDescending(c => c.CreateAt);
            return (filter, orderBy);
        }

    }
}
