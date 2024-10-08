﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Model.Account;

namespace Domain.Model.Highschool;
public class HighschoolPutModel : AccountPostModel
{
    [Required(ErrorMessage = "Name is required.")]
    public string Name { get; set; }
    [Required(ErrorMessage = "LocationDetail is required.")]
    public string LocationDetail { get; set; }
    [Required(ErrorMessage = "RegionId is required.")]
    public Guid RegionId { get; set; }
}
