using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Interface;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Persistence
{
    public class TokenValidationMiddleware : IMiddleware
    {
        private readonly IUnitOfWork _unitOfWork;

        public TokenValidationMiddleware(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var authorizationHeader = context.Request.Headers["Authorization"].FirstOrDefault();
            if (authorizationHeader != null && authorizationHeader.StartsWith("Bearer "))
            {
                var token = authorizationHeader.Substring("Bearer ".Length).Trim();

                if (!string.IsNullOrEmpty(token))
                {
                    try
                    {
                        var jwtTokenHandler = new JwtSecurityTokenHandler();
                        var jwtToken = jwtTokenHandler.ReadJwtToken(token);

                        var jti = jwtToken.Claims.First(claim => claim.Type == JwtRegisteredClaimNames.Jti).Value;
                        var isRevoked = await _unitOfWork.RefreshTokenRepository.SingleOrDefaultAsync(predicate: x=> x.JwtId.Equals(jti) && x.IsRevoked == true);

                        if (isRevoked != null)
                        {
                            context.Response.StatusCode = 401;
                            await context.Response.WriteAsync("Token has been revoked.");
                            return;
                        }
                    }
                    catch (Exception ex)
                    {
                        context.Response.StatusCode = 400;
                        await context.Response.WriteAsync($"Invalid token: {ex.Message}");
                        return;
                    }
                }
            }

            await next(context);
        }
    }
}
