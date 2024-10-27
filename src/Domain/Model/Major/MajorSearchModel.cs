﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Domain.Model.Major
{
    public class MajorSearchModel
    {
        [FromQuery(Name = "current-page")]
        public int? currentPage { get; set; }
        [FromQuery(Name = "page-size")]
        public int? pageSize { get; set; }
        [FromQuery(Name = "name")]
        public string? name { get; set; }
        [FromQuery(Name = "major-category-id")]
        public Guid? majorCategoryId { get; set; }
        [FromQuery(Name = "status")]
        public bool? status { get; set; }
    }
}
