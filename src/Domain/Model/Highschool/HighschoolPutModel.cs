﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model.Highschool;
public class HighschoolPutModel
{
    [Required(ErrorMessage = "Id is required.")]
    public Guid Id { get; set; }
    [Required(ErrorMessage = "Name is required.")]
    public string Name { get; set; }
    [Required(ErrorMessage = "LocationDetail is required.")]
    public string LocationDetail { get; set; }
}
