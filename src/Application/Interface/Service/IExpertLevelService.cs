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
    public interface IExpertLevelService
    {
        Task<ResponseModel> GetExpertLevelByIdAsync(int expertLevelId);
        Task<ResponseModel> CreateExpertLevelAsync(ExpertLevelPostModel postModel);
        Task<ResponseModel> UpdateExpertLevelAsync(int expertLevelId, ExpertLevelPutModel putModel);
        Task<ResponseModel> DeleteExpertLevelAsync(int expertLevelId);
    }
}
