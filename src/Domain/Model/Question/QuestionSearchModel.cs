using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Enum;
using Microsoft.AspNetCore.Mvc;

namespace Domain.Model.Question
{
    public class QuestionSearchModel
    {
        [FromQuery(Name = "personal-test-id")]
        [Required(ErrorMessage = "PersonalTestId is required")]
        public Guid PersonalTestId { get; set; }
        [FromQuery(Name = "current-page")]
        public int? Page { get; set; }
        [FromQuery(Name = "page-size")]
        public int? Size { get; set; }
        [FromQuery(Name = "Content")]
        public string? Content { get; set; }
        
    }
}
