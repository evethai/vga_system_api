using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using Application.Common.Utils;
using Application.Interface;
using Application.Interface.Service;
using AutoMapper;
using Domain.Entity;
using Domain.Enum;
using Domain.Model.Account;
using Domain.Model.Highschool;
using Domain.Model.Response;
using Domain.Model.Student;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Infrastructure.Persistence.Service;
public class StudentService : IStudentService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public StudentService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    public async Task<ResponseStudentModel> GetListStudentAsync(StudentSearchModel searchModel)
    {
        var (filter, orderBy) = _unitOfWork.StudentRepository.BuildFilterAndOrderBy(searchModel);
        var student = await _unitOfWork.StudentRepository
            .GetBySearchAsync(filter, orderBy,
            q => q.Include(s => s.Account)
                   .ThenInclude(a => a.Wallet),
            pageIndex: searchModel.currentPage,
            pageSize: searchModel.pageSize);

        var total = await _unitOfWork.StudentRepository.CountAsync(filter);
        var listStudent = _mapper.Map<List<StudentModel>>(student);
        return new ResponseStudentModel
        {
            total = total,
            currentPage = searchModel.currentPage,
            students = listStudent,
        };
    }

    public async Task<StudentModel> GetStudentByIdAsync(Guid StudentId)
    {
        var student = await _unitOfWork.StudentRepository.SingleOrDefaultAsync(predicate: c=>c.Id.Equals(StudentId),include:a=>a.Include(a=>a.Account).ThenInclude(a => a.Wallet));
        return _mapper.Map<StudentModel>(student);
    }

    public async Task<ResponseModel> UpdateStudentAsync(StudentPutModel putModel, Guid StudentId)
    {
        var exitStudent = await _unitOfWork.StudentRepository.GetByIdGuidAsync(StudentId);
        if (exitStudent == null)
        {
            throw new Exception("Student Id not found");
        }
        exitStudent.DateOfBirth = putModel.DateOfBirth;
        exitStudent.SchoolYears = putModel.SchoolYears;
        exitStudent.Gender = putModel.Gender;
        var exitAccount = await _unitOfWork.AccountRepository.GetByIdGuidAsync(exitStudent.AccountId);
        if (exitAccount == null)
        {
            throw new Exception("Student Account Id not found");
        }
        exitAccount.Name = putModel.Name;
        exitAccount.Phone = putModel.Phone;
        exitAccount.Email = putModel.Email;
        exitAccount.Password = PasswordUtil.HashPassword(putModel.Password);
        await _unitOfWork.AccountRepository.UpdateAsync(exitAccount);
        var result = await _unitOfWork.StudentRepository.UpdateAsync(exitStudent);
        await _unitOfWork.SaveChangesAsync();
        return new ResponseModel
        {
            Message = "Student Updated Successfully",
            IsSuccess = true,
            Data = putModel,
        };

    }
    public async Task<ResponseModel> CreateStudentAsync(StudentPostModel postModel)
    {
        
        RegisterAccountModel accountModel = new RegisterAccountModel(
            postModel.Name
            ,postModel.Email
            , postModel.Password
            , postModel.Phone);
        var AccountId = await _unitOfWork.AccountRepository.CreateAccountAndWallet(accountModel, RoleEnum.Student);
        var student = _mapper.Map<Student>(postModel);
        student.AccountId = AccountId;
        var result = await _unitOfWork.StudentRepository.AddAsync(student);
        await _unitOfWork.SaveChangesAsync();
        return new ResponseModel
        {
            Message = "Student Created Successfully",
            IsSuccess = true,
            Data = postModel,
        };
    }

    public async Task<ResponseModel> DeleteStudentAsync(Guid StudentId)
    {
        var exStudent = await _unitOfWork.StudentRepository.GetByIdGuidAsync(StudentId);
        var exAccountStudent = await _unitOfWork.AccountRepository.GetByIdGuidAsync(exStudent.AccountId);
        exAccountStudent.Status = AccountStatus.Blocked;
        await _unitOfWork.AccountRepository.UpdateAsync(exAccountStudent);
        await _unitOfWork.SaveChangesAsync();
        return new ResponseModel
        {
            Message = "Delete Student is Successfully",
            IsSuccess = true
        };
    }
    #region Import Students From Json Async
    public async Task<ResponseModel> ImportStudentsFromJsonAsync(StudentImportModel studentImportModel)
    {
        try
        {
            var students = JsonConvert.DeserializeObject<StudentJsonFileDataModel>(studentImportModel.stringJson);

            if (students == null || students.Data.Count == 0)
            {
                return new ResponseModel
                {
                    IsSuccess = false,
                    Message = "No students to import."
                };
            }
            //var roleId = await _unitOfWork.RoleRepository
            //    .SingleOrDefaultAsync(selector: x => x.Id, predicate: x => x.Name.Equals(RoleEnum.Student.ToString()));

            foreach (var studentImport in students.Data)
            {
                RegisterAccountModel accountModel = new RegisterAccountModel(studentImport.Name,studentImport.Email
                       , studentImport.Password
                       , studentImport.Phone);

                var student = _mapper.Map<Student>(studentImport);
                var AccountId = await _unitOfWork.AccountRepository.CreateAccountAndWallet(accountModel, RoleEnum.Student);

                student.Id = Guid.NewGuid();
                student.AccountId = AccountId;
                student.HighSchoolId = studentImportModel.highschoolId;
                student.SchoolYears = studentImportModel.schoolYear;

                await _unitOfWork.StudentRepository.AddAsync(student);
            }

            await _unitOfWork.SaveChangesAsync();

            return new ResponseModel
            {
                IsSuccess = true,
                Message = $"{students.Data.Count} students imported successfully."
            };
        }
        catch (Exception ex)
        {
            return new ResponseModel
            {
                IsSuccess = false,
                Message = $"An error occurred while importing students: {ex.Message}"
            };
        }
    }
    #endregion
}
