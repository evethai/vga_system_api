using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entity;
using Domain.Enum;

namespace Domain.Model.Test
{
    public class StudentChoiceModel
    {
        [Required(ErrorMessage = "Student id is require!")]
        public Guid StudentTestId { get; set; }

        public List<MajorOrOccupationModel> models { get; set; } = new List<MajorOrOccupationModel>();
    }

    public class MajorOrOccupationModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public int Rating { get; set; }
        //public StudentChoiceType Type { get; set; }
    }

    public class stChoiceModel
    {
        public Guid StudentTestId { get; set; }
        public virtual StudentTest StudentTest { get; set; } = null!;
        public Guid MajorOrOccupationId { get; set; }
        public string MajorOrOccupationName { get; set; } = null!;
        public int Rating { get; set; }
        public StudentChoiceType Type { get; set; }
        public string Image { get; set; } = null!;
    }
}
