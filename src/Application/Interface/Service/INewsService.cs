using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Model.News;
using Domain.Model.Response;

namespace Application.Interface.Service
{
    public interface INewsService
    {
        Task<ResponseNewsModel> GetListNewsAsync(NewsSearchModel searchModel);
        Task<NewsModel> GetNewsByIdAsync(Guid NewsId);
        Task<ResponseModel> CreateNewsAsync(NewsPostModel postModel);
        Task<ResponseModel> UpdateNewsAsync(NewsPutModel putModel, Guid Id);
        Task<ResponseModel> DeleteNewsAsync(Guid Id);

        Task<ResponseModel> CreateImageNewsAsync(Guid NewsId, List<ImageNewsPostModel> imageNews);
        Task<ResponseModel> DeleteImageNewsAsync(int Id);
        Task<ResponseModel> UpdateImageNewsAsync(ImageNewsPutModel imageNewsModel, int id);
    }
}
