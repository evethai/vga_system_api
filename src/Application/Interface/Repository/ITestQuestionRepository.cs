using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Domain.Entity;
using Domain.Model.Question;

namespace Application.Interface.Repository
{
    public interface ITestQuestionRepository : IGenericRepository<TestQuestion>
    {
        (Expression<Func<TestQuestion, bool>> filter, Func<IQueryable<TestQuestion>, IOrderedQueryable<TestQuestion>> orderBy)
        BuildFilterAndOrderBy(QuestionSearchModel searchModel);
    }
}
