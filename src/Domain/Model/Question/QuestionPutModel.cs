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
        [Required(ErrorMessage = "Content is required")]
        public string Content { get; set; } = string.Empty;
        [Required(ErrorMessage = "Group is required")]
        public QuestionGroup Group { get; set; }
        [RequiredPutModel()]
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

    public class RequiredPutModel : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var model = (QuestionPutModel)validationContext.ObjectInstance;

            if (model.Group != QuestionGroup.None)
            {
                model.Answers = null;
            }

            if (model.Group == QuestionGroup.None && (model.Answers == null || model.Answers.Count != 2))
            {
                return new ValidationResult("MBTI questions must have exactly 2 answers.");
            }

            else if (model.Group != QuestionGroup.None && (model.Answers != null || model.Answers.Any()))
            {
                return new ValidationResult("Holland code questions should not have answers.");
            }

            return ValidationResult.Success;
        }

    }
}
