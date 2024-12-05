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
        public Guid PersonalTestId { get; set; }
        public string PersonalTestName { get; set; } = string.Empty;
        public Guid PersonalGroupId { get; set; }
        public string PersonalGroupCode { get; set; } = string.Empty;
        public string PersonalGroupName { get; set; } = string.Empty;
        public string PersonalGroupDescription { get; set; } = string.Empty;
        public DateTime Date { get; set; }

    }

    public class StudentHistoryModel
    {
        public List<HistoryTestModel> HistoryTests { get; set; } = new List<HistoryTestModel>();
        public List<HistoryMajorModel> HistoryMajor { get; set; } = new List<HistoryMajorModel>();
        public List<MajorCategoryModel> MajorByHollandResult { get; set; } = new List<MajorCategoryModel>();
    }
    public class HistoryMajorModel
    {
        public Guid MajorId { get; set; }
        public string MajorName { get; set; } = string.Empty;
        public string? Image { get; set; } 
        public int Rating { get; set; }
    }
}
