using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Enum;

namespace Domain.Model.StudentChoice
{
    public class StudentChoicePostModel
    {
        [Required(ErrorMessage ="Student id need required!")]
        public Guid StudentId { get; set; }
        [Required(ErrorMessage = "Major or Occ id need required!")]
        public Guid MajorOrOccupationId { get; set; }
        [Required(ErrorMessage = "Rating is need required!")]
        public int Rating { get; set; }
        [Required(ErrorMessage ="is major is need required!")]
        public bool isMajor { get; set; }
    }
}
