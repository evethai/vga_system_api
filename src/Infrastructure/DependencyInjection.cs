using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Interface;
using Application.Interface.Repository;
using Application.Interface.Service;
using Infrastructure.Data;
using Infrastructure.Persistence;
using Infrastructure.Persistence.Repository;
using Infrastructure.Persistence.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<VgaDbContext>(options =>
            {
                options.UseSqlServer(connectionString);
            });

            //Declare Middleware
            //services.AddScoped<TokenValidationMiddleware>();

            //Declare UnitOfWork
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            //Declare Singleton for check multiple login
            //services.AddSingleton<ILockService, LockService>();

            //Declare Repository
            services.AddScoped<ITestRepository, TestRepository>();
            services.AddScoped<IRegionRepository, RegionRepository>();
            services.AddScoped<IHighschoolRepository, HighschoolRepository>();
            services.AddScoped<IStudentRepository, StudentRepository>();


            services.AddScoped<IResultMBTITestRepository, ResultMBTITestRepository>();


            //Declare Service
            //services.AddScoped<ITestService, TestService>();
            services.AddScoped<IRegionService, RegionService>();
            services.AddScoped<IHighschoolService, HighschoolService>();
            services.AddScoped<IStudentService, StudentService>();


            //Declare Email Service
            //services.AddTransient<IEmailService, EmailService>();
            //services.AddTransient<ICustomerOrderService, CustomerOrderService>();




            return services;
        }
    }
}
