using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Domain.Entity;
using Domain.Model.Highschool;
using Domain.Model.News;
using Domain.Model.Response;

namespace Application.Interface.Repository
{
    public interface INewsRepository :IGenericRepository<News>
    {
        (Expression<Func<News, bool>> filter, Func<IQueryable<News>, IOrderedQueryable<News>> orderBy) BuildFilterAndOrderBy(NewsSearchModel searchModel);
        Task<Boolean> DeleteOneImageNews(int id);
        Task<Boolean> DeleteAllImageNews(Guid NewId);
        Task<Boolean> CreateImageNews(Guid NewsId, List<ImageNewsPostModel> imageNews);
        Task<Boolean> UpdateImageNews(ImageNewsPutModel imageNews, int id);
        NewsModel HashTagNews(Guid NewsId);
        Task<ResponseModel> HashTagNotification(NewsPostModel postModel);
    }
}
