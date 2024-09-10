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
        ITestRepository TestRepository { get; }
        IRegionRepository RegionRepository { get; }
        IHighschoolRepository HighschoolRepository { get; }
        IStudentRepository StudentRepository { get; }
        IResultMBTITestRepository ResultMBTITestRepository { get; }
        int Save();
    }
}

