using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Model.Response;
using Domain.Model.University;

namespace Application.Interface.Service
{
    public interface IUniversityService
    {
        Task<ResponseUniversityModel> GetListUniversityAsync(UniversitySearchModel searchModel);
        Task<UniversityModel> GetUniversityByIdAsync(Guid Id);
        Task<ResponseModel> CreateUniversityAsync(UniversityPostModel postModel);
        Task<ResponseModel> UpdateUniversityAsync(UniversityPutModel putModel, Guid Id);
    }
}
