using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entity;

namespace Domain.Model.Test
{
    public class StudentTestModel
    {
        public int StudentId { get; set; }
        public int PersonalTestId { get; set; }
        public int PersonalGroupId { get; set; }
        public string JsonResult { get; set; } = string.Empty;
        public DateTime Date { get; set; }
    }
}
