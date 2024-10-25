﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Model.Highschool;

namespace Domain.Model.News
{
    public class NewsModel
    {
        public Guid Id { get; set; }
        public Guid UniversityId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public List<ImageNewsModel> ImageNews { get; set; }
    }
    public class ResponseNewsModel
    {
        public int? total { get; set; }
        public int? currentPage { get; set; }
        public List<NewsModel> _news { get; set; }
    }
}