﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Constants
{
    public static class CorsConstant
    {
        public const string PolicyName = "MyDefaultPolicy";
        public static readonly string[] AllowedOrigins = new[]
        {
            "http://localhost:3000",

        };
    }
}
