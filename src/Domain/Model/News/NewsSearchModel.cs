using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Domain.Model.News
{
    public class NewsSearchModel
    {
        [FromQuery(Name = "current-page")]
        public int? currentPage { get; set; }
        [FromQuery(Name = "page-size")]
        public int? pageSize { get; set; }
        [FromQuery(Name = "tilte")]
        public string? tilte { get; set; }
        [FromQuery(Name = "university-id")]
        public Guid? UniversityId { get; set; }
        [FromQuery(Name = "descending")]
        public bool? descending { get; set; } = false;
    }
}
