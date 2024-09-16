using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model.Test
{
    public class TestResultDataModel
    {
        public List<int>? AnswerIds { get; set; }
        public List<int> QuestionIds { get; set; } = new List<int>();
    }
}
