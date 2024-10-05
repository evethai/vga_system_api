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
            services.AddScoped<IPersonalTestRepository, PersonalTestRepository>();
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<ITestTypeRepository, TestTypeRepository>();
            services.AddScoped<IQuestionRepository, QuestionRepository>();
            services.AddScoped<IConsultantLevelRepository, ConsultantLevelRepository>();
            services.AddScoped<ITimeSlotRepository, TimeSlotRepository>();
            services.AddScoped<IConsultantRepository, ConsultantRepository>();
            services.AddScoped<IConsultationDayRepository, ConsultationDayRepository>();
            services.AddScoped<IConsultationTimeRepository, ConsultationTimeRepository>();
            services.AddScoped<IBookingRepository, BookingRepository>();    

            //Declare Service
            services.AddScoped<IStudentTestService, StudentTestService>();
            services.AddScoped<IRegionService, RegionService>();
            services.AddScoped<IHighschoolService, HighschoolService>();
            services.AddScoped<IStudentService, StudentService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<ITestTypeService, TestTypeService>();
            services.AddScoped<IQuestionService, QuestionService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IConsultantLevelService, ConsultantLevelService>();
            services.AddScoped<ITimeSlotService, TimeSlotService>();
            services.AddScoped<IConsultantService, ConsultantService>();
            services.AddScoped<IConsultationDayService, ConsultationDayService>();
            services.AddScoped<IConsultationTimeService, ConsultationTimeService>();
            services.AddScoped<IBookingService, BookingService>();

            //Declare Email Service
            //services.AddTransient<IEmailService, EmailService>();
            //services.AddTransient<ICustomerOrderService, CustomerOrderService>();
        }
    }
}
