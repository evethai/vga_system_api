using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model.News
{
    public class NewsPostModel
    {
        [Required(ErrorMessage = "UniversityId is required.")]
        public Guid UniversityId { get; set; }
        [Required(ErrorMessage = "Title is required.")]
        public string Title { get; set; } = string.Empty;
        [Required(ErrorMessage = "Content is required.")]
        public string Content { get; set; } = string.Empty;
        public List<ImageNewsPostModel>? ImageNews { get; set; }
    }
}
