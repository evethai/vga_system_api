﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
    public class PersonalGroup : BaseEntity
    {
        public Guid TestTypeId { get; set; }
        public TestType TestType { get; set; } = null!;
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string? About { get; set; }
        public bool Status { get; set; }
        public virtual ICollection<MajorPersonalityMatrix> MajorPersonalMatrixs { get; set; } = null!;
        public virtual ICollection<StudentTest> StudentTests { get; set; } = null!;

    }
}
