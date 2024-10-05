using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Model.ExpertLevel;
using Domain.Model.Response;
using Domain.Model.TimeSlot;

namespace Application.Interface.Service
{
    public interface IConsultantLevelService
    {
        Task<ResponseModel> GetConsultantLevelByIdAsync(int consultantLevelId);
        Task<ResponseModel> CreateConsultantLevelAsync(ConsultantLevelPostModel postModel);
        Task<ResponseModel> UpdateConsultantLevelAsync(ConsultantLevelPutModel putModel, int consultantLevelId);
        Task<ResponseModel> DeleteConsultantLevelAsync(int consultantLevelId);
    }
}
