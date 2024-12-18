﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Model.Consultant;

namespace Domain.Model.Major
{
    public class MajorViewModel
    {
        public Guid Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string AdmissionGroup { get; set; } = string.Empty;
        public bool Status { get; set; }
        public Guid MajorCategoryId { get; set; }
        public string MajorCategoryName { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;
        public bool IsCare { get; set; } = false;
        public int CareLevel { get; set; } = 0;
        public int NumberCare { get; set; } = 0;

        public List<OccupationByMajorIdModel>? Occupations { get; set; } = new List<OccupationByMajorIdModel>();
        public List<UniversityByMajorIdModel>? Universities { get; set; } = new List<UniversityByMajorIdModel>();
        public List<ConsultantOfMajorModel>? Consultatnts { get; set; } = new List<ConsultantOfMajorModel>();
    }

    public class OccupationByMajorIdModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;
    }
    public class UniversityByMajorIdModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;
    }
}
