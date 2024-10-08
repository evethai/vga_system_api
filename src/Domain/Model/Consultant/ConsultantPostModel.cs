using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model.Consultant
{
    public class ConsultantPostModel
    {
        public string Name { get; set; } = string.Empty;
        public int ConsultantLevelId { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string? Image_Url { get; set; }
        public string Description { get; set; } = string.Empty;
        public DateTime DoB { get; set; }
    }
}
