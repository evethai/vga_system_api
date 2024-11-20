using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Domain.Entity;
using Domain.Model.AdmissionInformation;
using Domain.Model.Test;

namespace Application.Interface.Repository
{
    public interface IAdmissionInformationRepository : IGenericRepository<AdmissionInformation> 
    {
        (Expression<Func<AdmissionInformation, bool>> filter, Func<IQueryable<AdmissionInformation>, IOrderedQueryable<AdmissionInformation>> orderBy) BuildFilterAndOrderBy(AdmissionInformationRattingModel model, StudentChoice stChoice);
        (Expression<Func<AdmissionInformation, bool>> filter, Func<IQueryable<AdmissionInformation>, IOrderedQueryable<AdmissionInformation>> orderBy) BuildFilterAndOrderByAdmissionInformation(AdmissionInformationSearchModel searchModel);
        Task<Boolean> CreateListAdmissionInformation(Guid UniversityId, List<AdmissionInformationPostModel> postModels);
        Task<Boolean> CheckAdmissionInformation(List<AdmissionInformationPutModel> putModel);
    }

}
