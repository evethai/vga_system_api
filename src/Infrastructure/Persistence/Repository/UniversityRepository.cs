using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Application.Common.Extensions;
using Application.Interface.Repository;
using Domain.Entity;
using Domain.Model.Major;
using Domain.Model.University;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repository
{
    public class UniversityRepository : GenericRepository<University>, IUniversityRepository
    {
        private readonly VgaDbContext _context;
        public UniversityRepository(VgaDbContext context) : base(context)
        {
            _context = context;
        }

        public (Expression<Func<University, bool>> filter, Func<IQueryable<University>, IOrderedQueryable<University>> orderBy) BuildFilterAndOrderBy(UniversitySearchModel searchModel)
        {
            Expression<Func<University, bool>> filter = p => true;
            Func<IQueryable<University>, IOrderedQueryable<University>> orderBy = null;
            if (!string.IsNullOrEmpty(searchModel.name))
            {
               filter = filter.And(p => p.Account.Name.Contains(searchModel.name));
            }
            if (!string.IsNullOrEmpty(searchModel.Description))
            {
                filter = filter.And(p => p.Description.Contains(searchModel.Description));
            }
            if (!string.IsNullOrEmpty(searchModel.Code))
            {
                filter = filter.And(p => p.Code.Contains(searchModel.Code));
            }
            if (!string.IsNullOrEmpty(searchModel.EstablishedYear))
            {
                filter = filter.And(p => p.EstablishedYear.Contains(searchModel.EstablishedYear));
            }
            if (searchModel.Status.HasValue)
            {
                filter = filter.And(p => p.Account.Status ==searchModel.Status);
            }
            if (searchModel.Type.HasValue)
            {
                filter = filter.And(p => p.Type == searchModel.Type);
            }
            return (filter, orderBy);
        }

        public Task<int> CreateUniversityLocation(Guid IdUniversity, List<UniversityLocationModel> universityLocations)
        {
            var exitUnversity = _context.University.Where(a=>a.Id.Equals(IdUniversity)).FirstOrDefault() ?? throw new Exception("Không tìm thấy ID trường đại học");          
            foreach (var location in universityLocations)
            {
                var exitRegion = _context.Region.Where(a => a.Id.Equals(location.RegionId)).FirstOrDefault() 
                    ?? throw new Exception("Không tìm thấy ID vùng");                
                UniversityLocation locations = new UniversityLocation
                {
                    UniversityId = IdUniversity,
                    RegionId = location.RegionId,
                    Address = location.Address
                };
                _context.UniversityLocation.Add(locations);
            }
            int numberLocation = universityLocations.Count();
            _context.SaveChanges();
            return Task.FromResult(numberLocation);
        }

        public Task<bool> DeleteUniversityLocation(int Id)
        {
            var exitlocation = _context.UniversityLocation.Where(a => a.Id.Equals(Id)).FirstOrDefault() ?? throw new Exception("Không tìm thấy ID");           
            _context.UniversityLocation.Remove(exitlocation);
            _context.SaveChanges();
            return Task.FromResult(true);
        }

        public Task<bool> UpdateUniversityLocation(int UniversityLocationId, UniversityLocationPutModel universityLocationPutModel)
        {
            var exitlocation  = _context.UniversityLocation
                .Where(a=>a.Id.Equals(UniversityLocationId)).FirstOrDefault() 
                ?? throw new Exception("Không tìm thấy ID");

            var exitRegion = _context.Region
                .Where(a => a.Id.Equals(universityLocationPutModel.RegionId)).FirstOrDefault()
                ?? throw new Exception("Không tìm thấy ID vùng");
            exitlocation.RegionId = universityLocationPutModel.RegionId;
            exitlocation.Address = universityLocationPutModel.Address;
            _context.UniversityLocation.Update(exitlocation);
            _context.SaveChanges();
            return Task.FromResult(true);
        }

        public async Task<IEnumerable<UniversityByMajorIdModel>> GetListUniversityByMajorId(Guid majorId)
        {
            var universities = await _context.Major
                .Where(m => m.Id == majorId)
                .SelectMany(m => m.AdmissionInformation)
                .OrderBy(m => m.University.Account.Name)
                .Select(ai => new UniversityByMajorIdModel
                {
                    Id = ai.UniversityId,
                    Name = ai.University.Account.Name,
                    Image = ai.University.Account.Image_Url,
                }
                )
                .Distinct()
                .ToListAsync();


            return universities;
        }

    }
}
