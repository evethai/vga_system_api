using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Interface.Repository;

namespace Application.Interface
{
    public interface IUnitOfWork : IDisposable
    {
        IRegionRepository RegionRepository { get; }
        IHighschoolRepository HighschoolRepository { get; }
        IStudentRepository StudentRepository { get; }
        IStudentTestRepository StudentTestRepository { get; }
        IPersonalTestRepository PersonalTestRepository { get; }
        IAccountRepository AccountRepository { get; }
        ITestTypeRepository TestTypeRepository { get; }
        IQuestionRepository QuestionRepository { get; }
        IUniversityRepository UniversityRepository { get; }
        IWalletRepository WalletRepository { get; }
        ITransactionRepository TransactionRepository { get; }
        IConsultantLevelRepository ConsultantLevelRepository { get; }
        ITimeSlotRepository TimeSlotRepository { get; }
        IConsultantRepository ConsultantRepository { get; }
        IConsultationDayRepository ConsultationDayRepository { get; }
        IConsultationTimeRepository ConsultationTimeRepository { get; }
        IBookingRepository BookingRepository { get; }
        IRefreshTokenRepository RefreshTokenRepository { get; }
        INotificationRepository NotificationRepository { get; }
        IAdmissionInformationRepository AdmissionInformationRepository { get; }
        INewsRepository NewsRepository { get; }
        IEntryLevelEducationRepository EntryLevelEducationRepository { get; }
        Task SaveChangesAsync();
    }
}

