﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Constants
{
    public static class JwtConstant
    {
        public const string SecretKey = "JWT_SECRET_KEY";
        public const string Issuer = "JWT_ISSUER";
        public const string TokenExpireInMinutes = "JWT_TOKEN_EXPIRE_TIME_IN_MINUTES";
    }
}
