using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Domain.Entity;
using Domain.Model.Highschool;
using Domain.Model.News;

namespace Application.Interface.Repository
{
    public interface INewsRepository :IGenericRepository<News>
    {
        (Expression<Func<News, bool>> filter, Func<IQueryable<News>, IOrderedQueryable<News>> orderBy) BuildFilterAndOrderBy(NewsSearchModel searchModel);
        Task<Boolean> DeleteImageNew(Guid NewsId);
        Task<Boolean> CreateImageNew(Guid NewsId, List<ImageNewsModel> imageNews);
        Task<Boolean> UpdateImageNew(ImageNewsModel imageNews);
    }
}
