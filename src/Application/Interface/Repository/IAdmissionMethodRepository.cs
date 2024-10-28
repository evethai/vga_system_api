using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Domain.Entity;
using Domain.Model.AdmissionInformation;

namespace Application.Interface.Repository
{
    public interface IAdmissionMethodRepository : IGenericRepository<AdmissionMethod>
    {
        (Expression<Func<AdmissionMethod, bool>> filter, Func<IQueryable<AdmissionMethod>, IOrderedQueryable<AdmissionMethod>> orderBy) BuildFilterAndOrderByAdmissionMethod(AdmissionMethodSearchModel searchModel);
    }
}
