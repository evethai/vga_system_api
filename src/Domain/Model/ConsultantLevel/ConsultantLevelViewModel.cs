﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model.ConsultantLevel
{
    public class ConsultantLevelViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public double PriceOnSlot { get; set; }
        public bool Status { get; set; }
    }
}
