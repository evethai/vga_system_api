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
        public int Point { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<QuestionModel> QuestionModels { get; set; } = new List<QuestionModel>();
    }
    public class ResponsePersonalTestModel
    {
        public int? total { get; set; }
        public int? currentPage { get; set; }
        public List<PersonalTestModel> questions { get; set; }
    }
}
