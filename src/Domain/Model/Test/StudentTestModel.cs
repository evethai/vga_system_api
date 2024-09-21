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
        public Guid StudentId { get; set; }
        public Guid PersonalTestId { get; set; }
        public Guid PersonalGroupId { get; set; }
        public string JsonResult { get; set; } = string.Empty;
        public DateTime Date { get; set; }
    }
}
