﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Interface.Repository;
using Domain.Entity;
using Infrastructure.Data;

namespace Infrastructure.Persistence.Repository
{
    public class UniversityRepository : GenericRepository<University>, IUniversityRepository
    {
        public UniversityRepository(VgaDbContext context) : base(context)
        {
        }
    }
}