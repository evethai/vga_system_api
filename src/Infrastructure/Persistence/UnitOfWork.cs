using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Interface;
using Application.Interface.Repository;
using AutoMapper;
using Infrastructure.Data;
using Infrastructure.Persistence.Repository;

namespace Infrastructure.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly VgaDbContext _context;

        private IRegionRepository _regionRepository;
        private IHighschoolRepository _highschoolRepository;
        private IStudentRepository _studentRepository;
        private IStudentTestRepository _studentTestRepository;
        private IPersonalTestRepository _personalTestRepository;
        private IAccountRepository _accountRepository;
        private IWalletRepository _walletRepository;
        private ITransactionRepository _transactionRepository;
        private IRoleRepository _roleRepository;
        private ITestTypeRepository _testTypeRepository;
        private IQuestionRepository _questionRepository;
        private IUniversityRepository _universityRepository;
        private IConsultantLevelRepository _consultantLevelRepository;
        private ITimeSlotRepository _timeSlotRepository;
        private IConsultantRepository _consultantRepository;
        private IConsultationDayRepository _consultationDayRepository;
        private IConsultationTimeRepository _consultationTimeRepository;
        private IBookingRepository _bookingRepository;
        private IRefreshTokenRepository _refreshTokenRepository;
        private INotificationRepository _notificationRepository;
        private INewsRepository _newsRepository;

        public UnitOfWork(VgaDbContext context)
        {
            _context = context;
        }

        public IRegionRepository RegionRepository => _regionRepository ??= new RegionRepository(_context);
        public IHighschoolRepository HighschoolRepository => _highschoolRepository ??= new HighschoolRepository(_context);
        public IStudentRepository StudentRepository => _studentRepository ??= new StudentRepository(_context);
        public IStudentTestRepository StudentTestRepository => _studentTestRepository ??= new StudentTestRepository(_context);
        public IPersonalTestRepository PersonalTestRepository => _personalTestRepository ??= new PersonalTestRepository(_context);
        public IAccountRepository AccountRepository => _accountRepository ??= new AccountRepository(_context);
        public IWalletRepository WalletRepository => _walletRepository ??= new WalletRepository(_context);
        public ITransactionRepository TransactionRepository => _transactionRepository ??= new TransactionRepository(_context);
        public IRoleRepository RoleRepository => _roleRepository ??= new RoleRepository(_context);
        public ITestTypeRepository TestTypeRepository => _testTypeRepository ??= new TestTypeRepository(_context);
        public IQuestionRepository QuestionRepository => _questionRepository ??= new QuestionRepository(_context);
        public IUniversityRepository UniversityRepository => _universityRepository ??= new UniversityRepository(_context);
        public IConsultantLevelRepository ConsultantLevelRepository => _consultantLevelRepository ??= new ConsultantLevelRepository(_context);
        public ITimeSlotRepository TimeSlotRepository => _timeSlotRepository ??= new TimeSlotRepository(_context);
        public IConsultantRepository ConsultantRepository => _consultantRepository ??= new ConsultantRepository(_context);
        public IConsultationDayRepository ConsultationDayRepository => _consultationDayRepository ??= new ConsultationDayRepository(_context);
        public IConsultationTimeRepository ConsultationTimeRepository => _consultationTimeRepository ??= new ConsultationTimeRepository(_context);
        public IBookingRepository BookingRepository => _bookingRepository ??= new BookingRepository(_context);
        public IRefreshTokenRepository RefreshTokenRepository => _refreshTokenRepository ??= new RefreshTokenRepository(_context);

        public INotificationRepository NotificationRepository => _notificationRepository ??= new NotificationRepository(_context);
        public INewsRepository NewsRepository => _newsRepository ??= new NewsRepository(_context);

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}

