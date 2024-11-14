using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Enum;

namespace Domain.Model.TestType
{
    public class TestTypeModel
    {
        public Guid Id { get; set; }
        public TestTypeCode TypeCode { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Point { get; set; }
        public bool Status { get; set; }
        public DateTime CreateAt { get; set; }
    }
}
