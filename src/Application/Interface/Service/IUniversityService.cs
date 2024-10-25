using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entity;
using Domain.Model.Response;
using Domain.Model.University;

namespace Application.Interface.Service
{
    public interface IUniversityService
    {
        Task<ResponseUniversityModel> GetListUniversityAsync(UniversitySearchModel searchModel);
        Task<UniversityModel> GetUniversityByIdAsync(Guid Id);
        Task<ResponseModel> DeleteUniversityAsync(Guid Id);
        Task<ResponseModel> CreateUniversityAsync(UniversityPostModel postModel);
        Task<ResponseModel> UpdateUniversityAsync(UniversityPutModel putModel, Guid Id);
        Task<ResponseModel> CreateUniversityLocationAsync(Guid Id, List<UniversityLocationModel> universityLocationModels); 
        Task<ResponseModel> UpdateUniversityLocationAsync(Guid Id, UniversityLocationPutModel universityLocationModels); 
        Task<ResponseModel> DeleteUniversityLocationAsync(Guid Id);
        
    }
}
