using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Application.Common.Extensions;
using Application.Interface.Repository;
using Domain.Entity;
using Domain.Model.Student;
using Domain.Model.Test;
using Infrastructure.Data;

namespace Infrastructure.Persistence.Repository
{
    public class AdmissionInformationRepository: GenericRepository<AdmissionInformation>, IAdmissionInformationRepository
    {

        public AdmissionInformationRepository(VgaDbContext context) : base(context)
        {
        }

        public (Expression<Func<AdmissionInformation, bool>> filter, Func<IQueryable<AdmissionInformation>, IOrderedQueryable<AdmissionInformation>> orderBy) BuildFilterAndOrderBy(AdmissionInformationModel model, List<Guid> majorIds)
        {
            Expression<Func<AdmissionInformation, bool>> predicate = p => true;
            Func<IQueryable<AdmissionInformation>, IOrderedQueryable<AdmissionInformation>> orderBy = null;

            if (model.AdmissionMethodId != Guid.Empty)
            {
                predicate = predicate.And(x => x.AdmissionMethodId == model.AdmissionMethodId);
            }

            if (model.TuitionFee > 0)
            {
                predicate = predicate.And(x => x.TuitionFee <= model.TuitionFee);
            }

            if (model.Year > 0)
            {
                predicate = predicate.And(x => x.Year == model.Year);
            }


            if (model.Region != Guid.Empty)
            {
                predicate = predicate.And(x => x.University.UniversityLocations.Any(l => l.RegionId == model.Region));
            }

            if (majorIds != null && majorIds.Any())
            {
                predicate = predicate.And(x => majorIds.Contains(x.MajorId));
            }

            return (predicate, orderBy);
        }


    }
}
