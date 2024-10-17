﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model.News
{
    public class ImageNewsModel
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public string DescriptionTitle { get; set; } = string.Empty;
        public int Status { get; set; }
    }
}
