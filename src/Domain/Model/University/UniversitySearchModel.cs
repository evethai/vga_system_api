using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Enum;
using Microsoft.AspNetCore.Mvc;

namespace Domain.Model.University
{
    public class UniversitySearchModel
    {
        [FromQuery(Name = "current-page")]
        public int? currentPage { get; set; }
        [FromQuery(Name = "page-size")]
        public int? pageSize { get; set; }
        [FromQuery(Name = "name")]
        public string? name { get; set; }
        [FromQuery(Name = "description")]
        public string? Description { get; set; }
        [FromQuery(Name = "code")]
        public string? Code { get; set; }
        [FromQuery(Name = "established-year")]
        public string? EstablishedYear { get; set; }
        [FromQuery(Name = "type")]
        public UniversityType? Type { get; set; }
        [FromQuery(Name = "status")]
        public AccountStatus? Status { get; set; }
        [FromQuery(Name = "descending")]
        public bool? descending { get; set; } = false;
    }
}
