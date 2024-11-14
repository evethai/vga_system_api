using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model.Admin
{
    public  class DashboardModel
    {
        public int NumberAccount { get; set; }
        public int TotalStudents { get; set; }
        public int TotalHighSchools { get; set; }
        public int TotalUniversities { get; set; }
        public int NumberOfTestsInDay { get; set; }
        public int NumberOfTestsInWeek { get; set; }
        public int NumberOfTestsInMonth { get; set; }


    }
}
