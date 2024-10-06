﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model.Consultant
{
    public class ConsultantViewModel
    {
        public Guid Id { get; set; }
        public Guid AccountId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string? Image_Url { get; set; }
        public string Description { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public bool Status { get; set; }
        public DateTime CreateAt { get; set; }
    }
}
