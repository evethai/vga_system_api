
using Application.Interface.Repository;
using Application.Interface;
using Infrastructure.Persistence.Repository;
using Infrastructure.Persistence;
using Application.Interface.Service;
using Infrastructure.Persistence.Service;
using Application.Common.Hubs;
using Application.Library;

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
            services.AddScoped<IUniversityService, UniversityService>();
            services.AddScoped<INewsService, NewsService>();
            services.AddScoped<IAdminService, AdminService>();
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<IEntryLevelEducationService, EntryLevelEducationService>();
            services.AddScoped<IMajorCategoryService, MajorCategoryService>();
            services.AddScoped<IMajorService, MajorService>();
            services.AddScoped<IOccupationalGroupService, OccupationalGroupService>();
            services.AddScoped<IOccupationService, OccupationService>();
            services.AddScoped<IWorkSkillsService, WorkSkillsService>();
            services.AddScoped<IAdmissionInformationService, AdmissionInformationService>();
            services.AddScoped<IAdmissionMethodService, AdmissionMethodService>();
            services.AddScoped<IPersonalTestService, PersonalTestService>();
            services.AddScoped<ICertificationService,CertificationService>();
            services.AddScoped<IStudentChoiceService, StudentChoiceService>();

            //declare hub
            services.AddSingleton<UserConnectionManager>();

            //declare payOS
            services.AddSingleton(x =>
                new PayOSService(
                    configuration["payOS:clientId"],
                    configuration["payOS:apiKey"],
                    configuration["payOS:checksumKey"]
                )
            );
        }
    }
}
