﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model.WorkSkills
{
    public class ResponseWorkSkillsModel
    {
        public int? total { get; set; }
        public int? currentPage { get; set; }
        public List<WorkSkillsViewModel> workSkills { get; set; }
    }
}
