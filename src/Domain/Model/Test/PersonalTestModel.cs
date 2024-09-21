using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entity;
using Domain.Model.Question;

namespace Domain.Model.Test
{
    public class PersonalTestModel
    {
        public Guid Id { get; set; }
        public Guid TestTypeId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<QuestionModel> QuestionModels { get; set; } = new List<QuestionModel>();
    }
}
