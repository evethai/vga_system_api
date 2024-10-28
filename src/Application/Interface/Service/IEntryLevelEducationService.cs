using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Model.EntryLevelEducation;
using Domain.Model.Response;
using Domain.Model.TimeSlot;

namespace Application.Interface.Service
{
    public interface IEntryLevelEducationService
    {
        Task<ResponseModel> GetEntryLevelByIdAsync(Guid entryLevelId);
        Task<ResponseModel> CreateEntryLevelAsync(EntryLevelEducationPostModel postModel);
        Task<ResponseModel> UpdateEntryLevelAsync(EntryLevelEducationPutModel putModel, Guid entryLevelId);
        Task<ResponseModel> DeleteEntryLevelAsync(Guid entryLevelId);
        Task<ResponseEntryLevelEducationModel> GetListEntryLevelsWithPaginateAsync(EntryLevelEducationSearchModel searchModel);
    }
}
