using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
    public class TestQuestion : BasicEntity
    {
        public Guid PersonalTestId { get; set; }
        public PersonalTest PersonalTest { get; set; } = null!;
        public int QuestionId { get; set; }
        public Question Question { get; set; } = null!;
        public bool Status { get; set; }
    }

}
