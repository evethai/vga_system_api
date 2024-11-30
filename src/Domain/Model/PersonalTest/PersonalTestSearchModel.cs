using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Domain.Model.PersonalTest
{
    public class PersonalTestSearchModel
    {
        [FromQuery(Name = "current-page")]
        public int? Page { get; set; }
        [FromQuery(Name = "page-size")]
        public int? Size { get; set; }
        [FromQuery(Name = "content")]
        public string? Name { get; set; }
    }
}
