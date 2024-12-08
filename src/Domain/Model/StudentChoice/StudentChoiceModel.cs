using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Enum;

namespace Domain.Model.StudentChoice
{
    public class StudentChoiceModel
    {
        public List<StudentCareModel>? listMajor { get; set; }
        public List<StudentCareModel>? listOccupation { get; set; }

    }

    public class StudentCareModel
    {
        public Guid MajorOrOccupationId { get; set; }
        public string MajorOrOccupationName { get; set; } = null!;
        public int Rating { get; set; }
    }
}
