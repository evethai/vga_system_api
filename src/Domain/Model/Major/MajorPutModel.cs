using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model.Major
{
    public class MajorPutModel
    {
        public string? Code { get; set; } = string.Empty;
        public string? Name { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;
        public Guid? MajorCategoryId { get; set; }
    }
}
