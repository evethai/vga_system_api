using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Enum;

namespace Domain.Model.Question
{
    public class QuestionPutModel
    {
        [Required(ErrorMessage = "Id is required")]
        public int Id { get; set; }
        [Required(ErrorMessage = "TestTypeId is required")]
        public string Content { get; set; } = string.Empty;
        [Required(ErrorMessage = "Group is required")]
        public QuestionGroup Group { get; set; }
        public bool status { get; set; }
        [RequiredIfMBTI()]
        public List<AnswerPutModel>? Answers { get; set; } = new List<AnswerPutModel>();
    }
    public class AnswerPutModel
    {
        [Required(ErrorMessage = "Answer content is required")]
        public int Id { get; set; }
        [Required(ErrorMessage = "Answer content is required")]
        public string Content { get; set; } = string.Empty;
        [Required(ErrorMessage = "Answer value is required")]
        public AnswerValue AnswerValue { get; set; }
    }
}
