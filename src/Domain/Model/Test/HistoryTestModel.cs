using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Model.Major;

namespace Domain.Model.Test
{
    public class HistoryTestModel
    {
        public int PersonalTestId { get; set; }
        public string PersonalTestName { get; set; } = string.Empty;
        public int PersonalGroupId { get; set; }
        public string PersonalGroupCode { get; set; } = string.Empty;
        public string PersonalGroupName { get; set; } = string.Empty;
        public string PersonalGroupDescription { get; set; } = string.Empty;
        public DateTime Date { get; set; }

    }
}
