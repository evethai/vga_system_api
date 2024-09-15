﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entity;

namespace Domain.Model.Test
{
    public class StudentTestResultModel
    {
        public int StudentId { get; set; }
        public int PersonalTestId { get; set; }
        public List<int> listQuestionId { get; set; } = new List<int>();
        public List<int>? listAnswerId { get; set; }
        public DateTime Date { get; set; }
    }
}
