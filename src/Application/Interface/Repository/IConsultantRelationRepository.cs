﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Domain.Entity;

namespace Application.Interface.Repository
{
    public interface IConsultantRelationRepository : IGenericRepository<ConsultantRelation>
    {
        Task AddRangeAsync(IEnumerable<ConsultantRelation> consultantRelations);
        void DeleteRange(IEnumerable<ConsultantRelation> relations);
    }
}
