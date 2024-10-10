
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
            services.AddScoped<TokenValidationMiddleware>();

            //Declare UnitOfWork
            services.AddScoped<IUnitOfWork, UnitOfWork>();

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
            services.AddScoped<IWalletService, WalletService>();
            services.AddScoped<ITransactionService, TransactionService>();
            services.AddScoped<IAdminService, AdminService>();

        }
    }
}
