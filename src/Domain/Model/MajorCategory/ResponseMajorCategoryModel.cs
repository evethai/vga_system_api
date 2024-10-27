using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Domain.Model.MajorCategory
{
    public class ResponseMajorCategoryModel
    {
        public int? total { get; set; }
        public int? currentPage { get; set; }
        public List<MajorCategoryViewModel> majorCategorys { get; set; }
    }
}
