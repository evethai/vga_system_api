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
        Task SaveChangesAsync();
    }
}

