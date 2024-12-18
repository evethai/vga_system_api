﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entity;

namespace Application.Interface.Repository
{
    public interface IOccupationSkillRepository : IGenericRepository<OccupationalSKills>
    {
        Task AddRangeAsync(IEnumerable<OccupationalSKills> occupationalSkills);
    }
}
