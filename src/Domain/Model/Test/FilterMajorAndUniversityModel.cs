﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entity;

namespace Domain.Model.Test
{
    public class FilterMajorAndUniversityModel
    {
        [Required(ErrorMessage = "Student choice is require!")]
        public StudentChoiceModel studentChoiceModel { get; set; }
        //public AdmissionInformationRattingModel? filterInfor { get; set; }

    }

    public class AdmissionInformationRattingModel
    {

        public Guid AdmissionMethodId { get; set; }
        public double TuitionFee { get; set; }
        public int Year { get; set; }
        public Guid Region { get; set; }
    }
}
