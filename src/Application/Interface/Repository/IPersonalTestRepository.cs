using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Domain.Entity;
using Domain.Model.PersonalTest;

namespace Application.Interface.Repository
{
    public interface IPersonalTestRepository : IGenericRepository<PersonalTest>
    {
        public (Expression<Func<PersonalTest, bool>> filter, Func<IQueryable<PersonalTest>, IOrderedQueryable<PersonalTest>> orderBy)
        BuildFilterAndOrderBy(PersonalTestSearchModel searchModel);
    }
}
