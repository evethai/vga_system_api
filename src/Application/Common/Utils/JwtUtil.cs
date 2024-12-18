﻿using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Domain.Entity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Application.Common.Utils
{
    public class JwtUtil
    {
        private JwtUtil()
        {
        }

        public static (string Token, string Id) GenerateJwtToken(Account account, Tuple<string, Guid> guidClaim, IConfiguration configuration)
        {
            JwtSecurityTokenHandler jwtHandler = new JwtSecurityTokenHandler();

            SymmetricSecurityKey secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSettings:SecretKey"]));
            var credentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256Signature);

            string issuer = configuration["JwtSettings:Issuer"];

            string jti = Guid.NewGuid().ToString();

            List<Claim> claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Jti, jti),
                new Claim(JwtRegisteredClaimNames.Email, account.Email),
                new Claim(ClaimTypes.Role, account.Role.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, account.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.PhoneNumber, account.Phone)
            };

            if (guidClaim != null)
            {
                claims.Add(new Claim(guidClaim.Item1, guidClaim.Item2.ToString()));
            }

            var expires = DateTime.Now.AddMinutes(configuration.GetValue<long>("JwtSettings:TokenExpireInMinutes"));

            var token = new JwtSecurityToken(issuer, null, claims, notBefore: DateTime.Now, expires, credentials);
            string tokenString = jwtHandler.WriteToken(token);

            return (Token: tokenString, Id: jti);
        }
    }

}

