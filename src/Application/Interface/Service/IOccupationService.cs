using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Model.Occupation;
using Domain.Model.Response;

namespace Application.Interface.Service
{
    public interface IOccupationService
    {
        Task<ResponseOccupationModel> GetListOccupationsWithPaginateAsync(OccupationSearchModel searchModel);
        Task<ResponseModel> GetOccupationByIdAsync(Guid occupationId, Guid studentId);
        Task<ResponseModel> CreateOccupationAsync(OccupationPostModel postModel);
        Task<ResponseModel> UpdateOccupationAsync(Guid occupationId, OccupationPutModel putModel);
        Task<ResponseModel> DeleteOccupationAsync(Guid occupationId);
    }
}
