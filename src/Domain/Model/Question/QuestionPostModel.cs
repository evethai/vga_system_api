using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entity;
using Domain.Enum;

namespace Domain.Model.Question
{
    public class QuestionPostModel
    {
        [Required(ErrorMessage = "TestTypeId is required")]
        public Guid TestTypeId { get; set; }

        [Required(ErrorMessage = "Content is required")]
        public string Content { get; set; } = string.Empty;
        [Required(ErrorMessage = "Group is required")]
        public QuestionGroup Group { get; set; }


        [RequiredIfMBTI()]
        public List<AnswerPostModel>? Answers { get; set; } = new List<AnswerPostModel>();
    }

    public class AnswerPostModel
    {
        [Required(ErrorMessage = "Answer content is required")]
        public string Content { get; set; } = string.Empty;

        [Required(ErrorMessage = "Answer value is required")]
        public AnswerValue AnswerValue { get; set; }
    }

    public class RequiredIfMBTI : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var model = (QuestionPostModel)validationContext.ObjectInstance;

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
