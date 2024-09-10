using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entity;
using Domain.Enum;

namespace Domain.Model.MBTI
{
    public class ResultModel
    {
        public Guid StudentId { get; set; }
        public int TestId { get; set; }
        public int PersonalityId { get; set; }
        public DateTime CreateAt { get; set; }
        public string description { get; set; } = string.Empty;
    }
}
