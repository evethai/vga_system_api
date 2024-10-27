using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Model.MajorCategory;
using Domain.Model.Response;

namespace Application.Interface.Service
{
    public interface IMajorCategoryService
    {
        Task<ResponseMajorCategoryModel> GetListMajorCategorysWithPaginateAsync(MajorCategorySearchModel searchModel);
        Task<ResponseModel> GetMajorCategoryByIdAsync(Guid majorCategoryID);
        Task<ResponseModel> CreateMajorCategoryAsync(MajorCategoryPostModel postModel);
        Task<ResponseModel> UpdateMajorCategoryAsync(MajorCategoryPutModel putModel, Guid majorCategoryID);
        Task<ResponseModel> DeleteMajorCategoryAsync(Guid majorCategoryID);
    }
}
