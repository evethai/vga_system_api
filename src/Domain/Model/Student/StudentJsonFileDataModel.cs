using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model.Student
{
    public class StudentJsonFileDataModel
    {
        public List<StudentJsonModel> Data { get; set; } = new List<StudentJsonModel>();
        public string FileName { get; set; } = string.Empty;
        public DateTime UploadDate { get; set; }
    }
}
