using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Model.Response;
using Domain.Model.WorkSkills;

namespace Application.Interface.Service
{
    public interface IWorkSkillsService
    {
        Task<ResponseWorkSkillsModel> GetListWorkSkillsWithPaginateAsync(WorkSkillsSearchModel searchModel);
        Task<ResponseModel> GetWorkSkillByIdAsync(Guid workSkillId);
        Task<ResponseModel> CreateWorkSkillAsync(WorkSkillsPostModel postModel);
        Task<ResponseModel> UpdateWorkSkillAsync(WorkSkillsPutModel putModel, Guid workSkillId);
        Task<ResponseModel> DeleteWorkSkillAsync(Guid workSkillId);
    }
}
