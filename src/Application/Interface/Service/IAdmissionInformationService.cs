using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Model.AdmissionInformation;
using Domain.Model.Response;
using Domain.Model.University;

namespace Application.Interface.Service
{
    public interface IAdmissionInformationService
    {
        Task<ResponseAdmissionInformationModel> GetListAdmissionInformationAsync(AdmissionInformationSearchModel searchModel);
        Task<ResponseModel> GetAdmissionInformationByIdAsync(int Id);
        Task<ResponseModel> DeleteAdmissionInformationAsync(int Id);
        Task<ResponseModel> CreateAdmissionInformationAsync(Guid UniversityId,List<AdmissionInformationPostModel> postModel);
        Task<ResponseModel> UpdateAdmissionInformationAsync(int Id, AdmissionInformationPutModel putModel);        
    }
}
