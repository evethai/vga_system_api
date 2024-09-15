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
        private RegionRepository _regionRepository;
        private HighschoolRepository _highSchoolRepository;
        private StudentRepository _studentRepository;
        private StudentTestRepository _studentTestRepository;
        private PersonalTestRepository _personalTestRepository;


        public UnitOfWork(VgaDbContext context)
        {
            _context = context;
        }


        public IRegionRepository RegionRepository
        {
            get
            {
                if (_regionRepository == null)
                {
                    _regionRepository = new RegionRepository(_context);
                }
                return _regionRepository;
            }
        }

        public IHighschoolRepository HighschoolRepository
        {
            get
            {
                if (_highSchoolRepository == null)
                {
                    _highSchoolRepository = new HighschoolRepository(_context);
                }
                return _highSchoolRepository;
            }
        }
        public IStudentRepository StudentRepository
        {
            get
            {
                if (_studentRepository == null)
                {
                    _studentRepository = new StudentRepository(_context);
                }
                return _studentRepository;
            }
        }
       

        public IStudentTestRepository StudentTestRepository
        {
            get
            {
                if (_studentTestRepository == null)
                {
                    _studentTestRepository = new StudentTestRepository(_context);
                }
                return _studentTestRepository;
            }
        }

        public IPersonalTestRepository PersonalTestRepository
        {
            get
            {
                if (_personalTestRepository == null)
                {
                    _personalTestRepository = new PersonalTestRepository(_context);
                }
                return _personalTestRepository;
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

