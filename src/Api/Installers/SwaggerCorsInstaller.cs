﻿using System.Text.Json.Serialization;
using Application.Common.Constants;
using Microsoft.OpenApi.Models;

namespace Api.Installers
{
    public class SwaggerCorsInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            // Swagger Configuration
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "VGA API", Version = "v1" });

                var securityScheme = new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    BearerFormat = "JWT",
                    Scheme = "Bearer",
                    Description = "Enter 'Bearer' [space] and then your token in the text input below.",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                };
                c.AddSecurityDefinition("Bearer", securityScheme);

                var securityRequirement = new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer",
                            },
                        },
                        new string[] { }
                    },
                };
                c.AddSecurityRequirement(securityRequirement);
            });

            services.AddControllers().AddJsonOptions(x =>
            x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles)
            .AddJsonOptions(option =>
            {
                //option.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.KebabCaseLower;
                option.JsonSerializerOptions.WriteIndented = true;
            });

            // CORS Configuration
            services.AddCors(options =>
            {
                //options.AddPolicy(name: CorsConstant.PolicyName,
                //policy => { policy.WithOrigins(CorsConstant.AllowedOrigins).AllowAnyHeader().AllowAnyMethod().AllowCredentials(); });
                options.AddPolicy(name: CorsConstant.PolicyName,
                        policy =>
                        {
                            policy.WithOrigins(CorsConstant.AllowedOrigins)
                            //policy.WithOrigins("*")
                                  .AllowAnyHeader()
                                  .AllowAnyMethod()
                                  .AllowCredentials();
                        });
            });
        }
    }
}
