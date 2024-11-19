using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model.MajorCategory
{
    public class MajorCategoryPutModel
    {
        public string? Name { get; set; }
        public string? Image { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;
    }
}
