using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model.Major
{
    public class MajorCategoryModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public List<MajorModel> Majors { get; set; } = new List<MajorModel>();
    }
}
