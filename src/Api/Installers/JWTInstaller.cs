using Api.Configurations;
using Application.Common.Constants;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Api.Installers
{
    public class JWTInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidIssuer = configuration["JwtSettings:Issuer"],
                    ValidateIssuer = true,
                    ValidateAudience = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey =
                        new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(configuration["JwtSettings:SecretKey"]))
                };

                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var authorizationHeader = context.Request.Headers["Authorization"].FirstOrDefault();
                        if (!string.IsNullOrEmpty(authorizationHeader) && authorizationHeader.StartsWith("Bearer "))
                        {
                            var token = authorizationHeader.Substring("Bearer ".Length).Trim(); 
                            var path = context.HttpContext.Request.Path;
                            if (path.StartsWithSegments("/notification_hub"))
                            {
                                context.Token = token;
                            }
                        }
                        return Task.CompletedTask;
                    },
                    OnAuthenticationFailed = context =>
                    {
                        context.Response.Headers["Token-Error"] = "Invalid token";
                        return Task.CompletedTask;
                    }
                };


            });
        }
    }


}
