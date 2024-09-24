﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Domain.Enum;

namespace Domain.Model.Student;
public class StudentImportModel
{
    public string stringJson { get; set; } = string.Empty;

    public Guid highschoolId { get; set; }
}
