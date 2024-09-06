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
        private readonly IMapper _mapper;
        private TestRepository _testRepository;
        private ResultMBTITestRepository _resultMBTITestRepository;


        public UnitOfWork(VgaDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public ITestRepository TestRepository
        {
            get
            {
                if (_testRepository == null)
                {
                    _testRepository = new TestRepository(_context);
                }
                return _testRepository;
            }
        }

        public IResultMBTITestRepository ResultMBTITestRepository
        {
            get
            {
                if (_resultMBTITestRepository == null)
                {
                    _resultMBTITestRepository = new ResultMBTITestRepository(_context);
                }
                return _resultMBTITestRepository;
            }
        }

        public int Save()
        {
            return _context.SaveChanges();
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

