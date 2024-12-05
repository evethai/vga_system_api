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
        public List<int> NumberOfMBTITestsInMonth { get; set; } = new List<int>();
        public List<int> NumberOfHollandTestsInMonth { get; set; } = new List<int>();
        public int TotalPointRechargeStudent { get; set; }
        public int TotalPointAdminTransferring { get; set; }


    }
}
