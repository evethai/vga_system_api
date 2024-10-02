using System.IdentityModel.Tokens.Jwt;
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

        public static string GenerateJwtToken(Account account, Tuple<string, Guid> guidClaim, IConfiguration configuration)
        {
            JwtSecurityTokenHandler jwtHandler = new JwtSecurityTokenHandler();


            SymmetricSecurityKey secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSettings:SecretKey"]));
            var credentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256Signature);


            string issuer = configuration["JwtSettings:Issuer"];

            List<Claim> claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, account.Email),
                new Claim(ClaimTypes.Role, account.Role.Name),
            };

            if (guidClaim != null)
            {
                claims.Add(new Claim(guidClaim.Item1, guidClaim.Item2.ToString()));
            }

            var expires = DateTime.Now.AddMinutes(configuration.GetValue<long>("JwtSettings:TokenExpireInMinutes"));

            var token = new JwtSecurityToken(issuer, null, claims, notBefore: DateTime.Now, expires, credentials);
            return jwtHandler.WriteToken(token);
        }
    }
}

