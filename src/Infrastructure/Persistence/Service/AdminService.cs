﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Interface;
using Application.Interface.Service;
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

        //dashboard admin
        public async Task<DashboardModel> GetDashboard()
        {
            var students = await _unitOfWork.StudentRepository.CountAsync();
            var highSchools = await _unitOfWork.HighschoolRepository.CountAsync();
            var universities = await _unitOfWork.UniversityRepository.CountAsync();
            var testsInDay = await _unitOfWork.StudentTestRepository.CountAsync(x => x.Date == DateTime.UtcNow.Date);
            var testsInWeek = await _unitOfWork.StudentTestRepository.CountAsync(x => x.Date >= DateTime.UtcNow.Date.AddDays(-7));
            var testsInMonth = await _unitOfWork.StudentTestRepository.CountAsync(x => x.Date >= DateTime.UtcNow.Date.AddMonths(-1));

            return new DashboardModel
            {
                TotalStudents = students,
                TotalHighSchools = highSchools,
                TotalUniversities = universities,
                NumberOfTestsInDay = testsInDay,
                NumberOfTestsInWeek = testsInWeek,
                NumberOfTestsInMonth = testsInMonth
            };

        }
    }
}
