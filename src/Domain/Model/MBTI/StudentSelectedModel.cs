using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entity;
using Microsoft.AspNetCore.Mvc;

namespace Domain.Model.MBTI
{
    public class StudentSelectedModel
    {
        public Guid StudentId { get; set; }
        public int TestId { get; set; }
        public required List<int> AnswerId { get; set; }
        public DateTime CreateAt { get; set; }
    }
} 
