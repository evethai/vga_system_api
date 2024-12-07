using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Application.Common.Extensions;
using Application.Interface.Repository;
using Domain.Entity;
using Domain.Model.Question;
using Infrastructure.Data;

namespace Infrastructure.Persistence.Repository
{
    public class TestQuestionRepository : GenericRepository<TestQuestion>, ITestQuestionRepository
    {
        public TestQuestionRepository(VgaDbContext context) : base(context)
        {

        }
        public (Expression<Func<TestQuestion, bool>> filter, Func<IQueryable<TestQuestion>, IOrderedQueryable<TestQuestion>> orderBy)
        BuildFilterAndOrderBy(QuestionSearchModel searchModel)
        {
            Expression<Func<TestQuestion, bool>> filter = p => p.PersonalTestId.Equals(searchModel.PersonalTestId) && p.Status == true && p.Question.Status == true ;
            Func<IQueryable<TestQuestion>, IOrderedQueryable<TestQuestion>> orderBy = null;
            if (!string.IsNullOrEmpty(searchModel.Content))
            {
                filter = filter.And(c => c.Question.Content.Contains(searchModel.Content));
            }
            orderBy = q => q.OrderByDescending(c => c.Question.Group);
            return (filter, orderBy);
        }
    }
}
