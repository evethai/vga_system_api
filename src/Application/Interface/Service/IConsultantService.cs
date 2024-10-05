using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Model.Consultant;
using Domain.Model.Response;

namespace Application.Interface.Service
{
    public interface IConsultantService
    {
        Task<ResponseModel> GetConsultantByIdAsync(Guid consultantId);
        Task<ResponseModel> CreateConsultantAsync(ConsultantPostModel postModel);
        Task<ResponseModel> UpdateConsultantAsync(Guid consultantId, ConsultantPutModel putModel);
        Task<ResponseModel> DeleteConsultantAsync(Guid consultantId);
        Task<ResponseConsultantModel> GetListConsultantsWithPaginateAsync(ConsultantSearchModel searchModel);
    }
}
