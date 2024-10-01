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
        IRoleRepository RoleRepository { get; }
        ITestTypeRepository TestTypeRepository { get; }
        IQuestionRepository QuestionRepository { get; }
        IExpertLevelRepository ExpertLevelRepository { get; }
        ITimeSlotRepository TimeSlotRepository { get; }
        ICareerExpertRepository CareerExpertRepository { get; }

        Task SaveChangesAsync();
    }
}

