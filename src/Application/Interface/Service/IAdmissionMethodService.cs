using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Model.AdmissionInformation;
using Domain.Model.Response;

namespace Application.Interface.Service
{
    public interface IAdmissionMethodService
    {
        Task<ResponseAdmissionMethodModel> GetListAdmissionMethodAsync(AdmissionMethodSearchModel searchModel);
        Task<ResponseModel> GetAdmissionMethodById(Guid Id);
        Task<ResponseModel> DeleteAdmissionMethodAsync(Guid Id);
        Task<ResponseModel> CreateAdmissionMethodAsync(AdmissionMethodPostModel postModel);
        Task<ResponseModel> UpdateAdmissionMethodAsync(Guid Id, AdmissionMethodPutModel putModel);
    }
}
