using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entity
{
    public class News : BaseEntity
    {
        public Guid UniversityId { get; set; }
        public University University { get; set; } = null!;
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public virtual ICollection<ImageNews> ImageNews { get; set; } = null!;
    }
}
