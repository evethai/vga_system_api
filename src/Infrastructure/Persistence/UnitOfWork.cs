﻿using System;
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
        private IRoleRepository _roleRepository;
        private ITestTypeRepository _testTypeRepository;
        private IQuestionRepository _questionRepository;
        private IExpertLevelRepository _expertLevelRepository;
        private ITimeSlotRepository _timeSlotRepository;
        private ICareerExpertRepository _careerExpertRepository;

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
        public IRoleRepository RoleRepository => _roleRepository ??= new RoleRepository(_context);
        public ITestTypeRepository TestTypeRepository => _testTypeRepository ??= new TestTypeRepository(_context);
        public IQuestionRepository QuestionRepository => _questionRepository ??= new QuestionRepository(_context);
        public IExpertLevelRepository ExpertLevelRepository => _expertLevelRepository ??= new ExpertLevelRepository(_context);
        public ITimeSlotRepository TimeSlotRepository => _timeSlotRepository ??= new TimeSlotRepository(_context);
        public ICareerExpertRepository CareerExpertRepository => _careerExpertRepository ??= new CareerExpertRepository(_context);

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

