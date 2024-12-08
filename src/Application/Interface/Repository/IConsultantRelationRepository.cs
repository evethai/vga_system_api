using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Domain.Entity;

namespace Application.Interface.Repository
{
    public interface IConsultantRelationRepository : IGenericRepository<ConstantRelation>
    {
        Task AddRangeAsync(IEnumerable<ConstantRelation> consultantRelations);
        void DeleteRange(IEnumerable<ConstantRelation> relations);
    }
}
