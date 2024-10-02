﻿
using Application.Interface.Repository;
using Application.Interface;
using Infrastructure.Persistence.Repository;
using Infrastructure.Persistence;
using Application.Interface.Service;
using Infrastructure.Persistence.Service;

namespace Api.Installers
{
    public class DeclareInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            //Declare Middleware
            //services.AddScoped<TokenValidationMiddleware>();

            //Declare UnitOfWork
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            //Declare Singleton for check multiple login
            //services.AddSingleton<ILockService, LockService>();

            //Declare Repository
            services.AddScoped<IStudentTestRepository, StudentTestRepository>();
            services.AddScoped<IRegionRepository, RegionRepository>();
            services.AddScoped<IHighschoolRepository, HighschoolRepository>();
            services.AddScoped<IStudentRepository, StudentRepository>();
            services.AddScoped<IWalletRepository, WalletRepository>();
            services.AddScoped<ITransactionRepository, TransactionRepository>();
            


            //Declare Service
            services.AddScoped<IStudentTestService, StudentTestService>();
            services.AddScoped<IRegionService, RegionService>();
            services.AddScoped<IHighschoolService, HighschoolService>();
            services.AddScoped<IStudentService, StudentService>();
            services.AddScoped<IWalletService, WalletService>();
            //services.AddScoped<ITransactionService, ITransactionService>();
            

           


            //Declare Email Service
            //services.AddTransient<IEmailService, EmailService>();
            //services.AddTransient<ICustomerOrderService, CustomerOrderService>();
        }
    }
}
