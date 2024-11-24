using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Model.Major;

namespace Domain.Model.Test
{
    public class ResultAfterRatingModel
    {
        public Guid MajorId { get; set; }
        public string MajorName { get; set; }
        public string? Image { get; set; }
        public List<OccupationByMajorIdModel>? _occupations { get; set; } = new List<OccupationByMajorIdModel>();
        public List<UniversityByMajorIdModel>? _universities { get; set; } = new List<UniversityByMajorIdModel>();
    }
}
