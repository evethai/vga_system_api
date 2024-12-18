﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Enum;
using Microsoft.AspNetCore.Mvc;

namespace Domain.Entity
{
    public class Student : BaseEntity
    {
        public Guid AccountId { get; set; }
        public virtual Account Account { get; set; } = null!;
        public Guid HighSchoolId { get; set; }
        public HighSchool HighSchool { get; set; } = null!;
        //public string Name { get; set; } = string.Empty;
        public bool Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int SchoolYears { get; set; }
        public virtual ICollection<StudentTest> StudentTests { get; set; } = null!;
        public virtual ICollection<Booking> Bookings { get; set; } = null!;
        public virtual ICollection<StudentChoice> StudentChoices { get; set; } = null!;

    }

}
