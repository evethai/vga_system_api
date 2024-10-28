using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entity;
using Domain.Model.OccupationalGroup;
using Domain.Model.Response;

namespace Application.Interface.Service
{
    public interface IOccupationalGroupService
    {
        Task<ResponseOccupationalGroupModel> GetListOccupationalGroupsWithPaginateAsync(OccupationalGroupSearchModel searchModel);
        Task<ResponseModel> GetOccupationalGroupByIdAsync(Guid occupationalGroupId);
        Task<ResponseModel> CreateOccupationalGroupAsync(OccupationalGroupPostModel postModel);
        Task<ResponseModel> UpdateOccupationalGroupAsync(OccupationalGroupPutModel putModel, Guid occupationalGroupId);
        Task<ResponseModel> DeleteOccupationalGroupAsync(Guid occupationalGroupId);
    }
}
