using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Model.Major;
using Domain.Model.Response;

namespace Application.Interface.Service
{
    public interface IMajorService
    {
        Task<ResponseMajorModel> GetListMajorsWithPaginateAsync(MajorSearchModel searchModel);
        Task<ResponseModel> GetMajorByIdAsync(Guid majorId);
        Task<ResponseModel> CreateMajorAsync(MajorPostModel postModel);
        Task<ResponseModel> UpdateMajorAsync(MajorPutModel putModel, Guid majorId);
        Task<ResponseModel> DeleteMajorAsync(Guid majorId);
    }
}
