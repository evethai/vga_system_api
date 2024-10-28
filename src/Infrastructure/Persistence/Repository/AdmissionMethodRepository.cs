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
using Infrastructure.Data;

namespace Infrastructure.Persistence.Repository
{
    public class AdmissionMethodRepository : GenericRepository<AdmissionMethod>, IAdmissionMethodRepository
    {
        private readonly VgaDbContext _context;
        public AdmissionMethodRepository(VgaDbContext context) : base(context)
        {
            _context = context;
        }
        public (Expression<Func<AdmissionMethod, bool>> filter, Func<IQueryable<AdmissionMethod>, IOrderedQueryable<AdmissionMethod>> orderBy) BuildFilterAndOrderByAdmissionMethod(AdmissionMethodSearchModel searchModel)
        {
            Expression<Func<AdmissionMethod, bool>> predicate = p => true;
            Func<IQueryable<AdmissionMethod>, IOrderedQueryable<AdmissionMethod>> orderBy = null;
            if (searchModel.Status.HasValue)
            {
                predicate = predicate.And(x => x.Status == searchModel.Status);
            }
            if (!string.IsNullOrEmpty(searchModel.Name))
            {
                predicate = predicate.And(p => p.Name.Contains(searchModel.Name));
            }
            if (!string.IsNullOrEmpty(searchModel.Description))
            {
                predicate = predicate.And(p => p.Description.Contains(searchModel.Description));
            }
            return (predicate, orderBy);
        }

    }
}
