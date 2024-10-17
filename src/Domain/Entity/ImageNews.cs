using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
    public class ImageNews : BasicEntity
    {
        public Guid NewsId { get; set; }
        public News News { get; set; } = null!;
        public string ImageUrl { get; set; } = string.Empty;
        public string DescriptionTitle { get; set; } = string.Empty;  
        public bool Status { get; set; }
    }
}
