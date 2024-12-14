using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Interface;
using Application.Interface.Service;
using Domain.Enum;
using Domain.Model.Admin;

namespace Infrastructure.Persistence.Service
{
    public class AdminService : IAdminService
    {
        private readonly IUnitOfWork _unitOfWork;
        public AdminService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<DashboardModel> GetDashboard()
        {
            var students = await _unitOfWork.StudentRepository.CountAsync();
            var highSchools = await _unitOfWork.HighschoolRepository.CountAsync();
            var universities = await _unitOfWork.UniversityRepository.CountAsync();
            var numberAcc = await _unitOfWork.AccountRepository.CountAsync(a => a.Role != RoleEnum.Admin);
            var testsInDay = await _unitOfWork.StudentTestRepository.CountAsync(x => x.Date == DateTime.UtcNow.AddHours(7).Date);
            var testsInWeek = await _unitOfWork.StudentTestRepository.CountAsync(x => x.Date >= DateTime.UtcNow.AddHours(7).Date.AddDays(-7));
            //var testsInMonth = await _unitOfWork.StudentTestRepository.CountAsync(x => x.Date >= DateTime.UtcNow.AddHours(7).Date.AddMonths(-1));


            var mbtiTestsInMonth = new List<int>();
            var hollandTestsInMonth = new List<int>();
            int monthNow = DateTime.UtcNow.AddHours(7).Month;
            //for from this month to 1 if after this month not have test return 0
            for (int i = monthNow; i > 0; i--)
            {
                var test = await _unitOfWork.StudentTestRepository.CountAsync(x => x.Date.Month == DateTime.UtcNow.AddHours(7).Date.AddMonths(-i+1).Month && x.PersonalTest.TestType.TypeCode == TestTypeCode.MBTI);
                mbtiTestsInMonth.Add(test);
                var test1 = await _unitOfWork.StudentTestRepository.CountAsync(x => x.Date.Month == DateTime.UtcNow.AddHours(7).Date.AddMonths(-i + 1).Month && x.PersonalTest.TestType.TypeCode == TestTypeCode.Holland);
                hollandTestsInMonth.Add(test1);
            }



            var totalPoint = await _unitOfWork.TransactionRepository.GetListAsync(predicate: x => x.Wallet.Account.Role == RoleEnum.Student &&
                                                                                             x.TransactionType == TransactionType.Recharge &&
                                                                                             x.TransactionDateTime >= DateTime.UtcNow.AddHours(7).Date.AddMonths(-1),
                                                                                  selector: x => x.GoldAmount);
            var totalPointStudent = totalPoint.Sum();

            var numberAdminTransferring = await _unitOfWork.TransactionRepository.GetListAsync(predicate: x => x.Wallet.Account.Role == RoleEnum.Admin &&
                                                                                                    x.TransactionType == TransactionType.Transferring &&
                                                                                                    x.TransactionDateTime >= DateTime.UtcNow.AddHours(7).Date.AddMonths(-1),
                                                                                                    selector: x => x.GoldAmount);
            var totalPointAdminTransferring = numberAdminTransferring.Sum();
            return new DashboardModel
            {
                NumberAccount = numberAcc,
                TotalStudents = students,
                TotalHighSchools = highSchools,
                TotalUniversities = universities,
                NumberOfTestsInDay = testsInDay,
                NumberOfTestsInWeek = testsInWeek,
                NumberOfMBTITestsInMonth = mbtiTestsInMonth,
                NumberOfHollandTestsInMonth = hollandTestsInMonth,
                TotalPointRechargeStudent = totalPointStudent,
                TotalPointAdminTransferring = totalPointAdminTransferring
            };

        }

        public async Task<DashboardUniversityModel> GetUniversityDashBoard(Guid universityId)
        {
            var numberOfConsultant = await _unitOfWork.ConsultantRelationRepository.CountAsync(x => x.UniversityId == universityId);
            var totalReceive = await _unitOfWork.TransactionRepository.GetListAsync(predicate: x=>x.Wallet.Account.University.Id == universityId &&
                                                                                                x.TransactionType == TransactionType.Receiving, selector : x=>x.GoldAmount);
            var sumReceive = totalReceive.Sum();

            var totalTransfer = await _unitOfWork.TransactionRepository.GetListAsync(predicate: x=>x.Wallet.Account.University.Id == universityId &&
                                                                                                x.TransactionType == TransactionType.Transferring, selector : x=>x.GoldAmount);
            var sumTransfer = totalTransfer.Sum();

            List<int> listTransferEachMonth = new List<int>();
            int monthNow = DateTime.UtcNow.AddHours(7).Month;
            for (int i = monthNow; i > 0; i--)
            {
                var numberOfEachMonth = await _unitOfWork.TransactionRepository.GetListAsync(predicate: x => x.Wallet.Account.University.Id == universityId &&
                                                                                                x.TransactionType == TransactionType.Transferring &&
                                                                                                x.TransactionDateTime.Month == DateTime.UtcNow.AddHours(7).Date.AddMonths(-i + 1).Month, selector: x => x.GoldAmount);
                listTransferEachMonth.Add(numberOfEachMonth.Sum());
            }


            return new DashboardUniversityModel
            {
                NumberConsultant = numberOfConsultant,
                TotalPointReceive = sumReceive,
                TotalPointTransfer = sumTransfer,
                NumberOfPointTransferringInMonth = listTransferEachMonth
            };
        }
    }
}
