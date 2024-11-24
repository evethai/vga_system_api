using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Enum;
using Domain.Model.Question;

namespace Domain.Model.PersonalTest
{
    public class PersonalTestPostModel
    {
        [Required(ErrorMessage ="Test type id is require!")]
        public Guid TestTypeId { get; set; }
        [Required(ErrorMessage = "Name is require!")]
        public string Name { get; set; } = string.Empty;
        [Required(ErrorMessage = "Description is require!")]
        public string Description { get; set; } = string.Empty;
        [Required(ErrorMessage = "Json data is require!")]
        public string JsonData { get; set; } = string.Empty;
    }

    public class DataQuestionMBTIModel
    {
        public string Content { get; set; } = string.Empty;
        public string Answer1 { get; set; } = string.Empty;
        public AnswerValue Value1 { get; set; }
        public string Answer2 { get; set; } = string.Empty;
        public AnswerValue Value2 { get; set; }
    }
    public class DataQuestionHollandModel
    {
        public string Content { get; set; } = string.Empty;
        public QuestionGroup Group { get; set; }
    }



}
