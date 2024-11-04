using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Application.Common.Extensions;
using Application.Interface.Repository;
using Domain.Entity;
using Domain.Model.AdmissionInformation;
using Domain.Model.Student;
using Domain.Model.Test;
using Infrastructure.Data;

namespace Infrastructure.Persistence.Repository
{
    public class AdmissionInformationRepository: GenericRepository<AdmissionInformation>, IAdmissionInformationRepository
    {
        private readonly VgaDbContext _context;
        public AdmissionInformationRepository(VgaDbContext context) : base(context)
        {
            _context = context;
        }
        public (Expression<Func<AdmissionInformation, bool>> filter, Func<IQueryable<AdmissionInformation>, IOrderedQueryable<AdmissionInformation>> orderBy) BuildFilterAndOrderBy(AdmissionInformationRattingModel model, StudentChoice stChoice)
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

            if (stChoice != null )
            {
                predicate = predicate.And(x => x.MajorId == stChoice.MajorOrOccupationId);
            }

            return (predicate, orderBy);
        }
        public (Expression<Func<AdmissionInformation, bool>> filter, Func<IQueryable<AdmissionInformation>, IOrderedQueryable<AdmissionInformation>> orderBy) BuildFilterAndOrderByAdmissionInformation(AdmissionInformationSearchModel searchModel)
        {
            Expression<Func<AdmissionInformation, bool>> filter = p => true;
            Func<IQueryable<AdmissionInformation>, IOrderedQueryable<AdmissionInformation>> orderBy = null;

            if (searchModel.AdmissionMethodId.HasValue)
            {
                filter = filter.And(x => x.AdmissionMethodId == searchModel.AdmissionMethodId);
            }
            if (searchModel.MajorId.HasValue)
            {
                filter = filter.And(x => x.MajorId == searchModel.MajorId);
            }
            if (searchModel.UniversityId.HasValue)
            {
                filter = filter.And(x => x.UniversityId == searchModel.UniversityId);
            }
            if (searchModel.TuitionFee.HasValue)
            {
                filter = filter.And(x => x.TuitionFee == searchModel.TuitionFee);
            }
            if (searchModel.Year.HasValue)
            {
                filter = filter.And(x => x.Year == searchModel.Year);
            }  
            if (searchModel.Status.HasValue)
            {
                    filter = filter.And(x => x.Status == searchModel.Status);
            }
            if (searchModel.QuantityTarget.HasValue)
            {
                    filter = filter.And(x => x.QuantityTarget == searchModel.QuantityTarget);
            }
            return (filter, orderBy);
        }
        public Task<bool> CheckAdmissionInformation(AdmissionInformationPutModel putModel)
        {
           var checkMajor = _context.Major.Where(a=>a.Id.Equals(putModel.MajorId)).FirstOrDefault();
           var checkMethod = _context.AdmissionMethod.Where(a => a.Id.Equals(putModel.AdmissionMethodId)).FirstOrDefault();
           if (checkMajor == null || checkMethod == null)
           {
                return Task.FromResult(false);
                
           }
           return Task.FromResult(true);         
        }
        public Task<bool> CreateListAdmissionInformation(Guid UniversityId, List<AdmissionInformationPostModel> postModels)
        {
            var exitUniversity =  _context.University.Where(a=>a.Id.Equals(UniversityId)).FirstOrDefault();
            if (exitUniversity == null)
            {
                throw new Exception("University Id is not found");
            }
            foreach (var postModel in postModels)
            {
                var exitAdmissionMethod = _context.AdmissionMethod.Where(i=>i.Id.Equals(postModel.AdmissionMethodId)).FirstOrDefault();
                var exitMajor = _context.Major.Where(a=>a.Id.Equals(postModel.MajorId)).FirstOrDefault();
                if (exitAdmissionMethod == null || exitMajor == null)
                {
                    throw new Exception("Method Id or Major Id is not found");
                }
                AdmissionInformation info = new AdmissionInformation
                {
                    UniversityId = UniversityId,
                    MajorId = postModel.MajorId,
                    AdmissionMethodId = postModel.AdmissionMethodId,
                    QuantityTarget = postModel.QuantityTarget,
                    TuitionFee = postModel.TuitionFee,
                    Status = true,
                    Year = postModel.Year,
                };
                _context.AdmissionInformation.AddAsync(info);              
            }
            _context.SaveChanges();
            return Task.FromResult(true);
        }     
    }
}
