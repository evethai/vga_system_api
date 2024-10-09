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
        var student = await _unitOfWork.StudentRepository.GetByIdGuidAsync(StudentId);
        return _mapper.Map<StudentModel>(student);
    }

    public async Task<ResponseModel> UpdateStudentAsync(StudentPutModel putModel, Guid StudentId)
    {
        var exitStudent = await _unitOfWork.StudentRepository.GetByIdGuidAsync(StudentId);
        if (exitStudent == null)
        {
            throw new Exception("Student Id not found");
        }
        var exitAccount = await _unitOfWork.AccountRepository.GetByIdGuidAsync(exitStudent.AccountId);
        exitAccount.Phone = putModel.Phone;
        exitAccount.Email = putModel.Email;
        exitAccount.Password = PasswordUtil.HashPassword(putModel.Password);
        await _unitOfWork.AccountRepository.UpdateAsync(exitAccount);
        var result = await _unitOfWork.StudentRepository.UpdateAsync(exitStudent);
         _unitOfWork.SaveChangesAsync();
        return new ResponseModel
        {
            Message = "Student Updated Successfully",
            IsSuccess = true,
            Data = result,
        };

    }
    public async Task<ResponseModel> CreateStudentAsync(StudentPostModel postModel)
    {
        var student = _mapper.Map<Student>(postModel);
        RegisterAccountModel accountModel = new RegisterAccountModel(postModel.Email
            , postModel.Password
            , postModel.Phone);
        var AccountId = await _unitOfWork.AccountRepository.CreateAccountAndWallet(accountModel,RoleEnum.Student);
        student.AccountId = AccountId;
        var result = await _unitOfWork.StudentRepository.AddAsync(student);
         await _unitOfWork.SaveChangesAsync();
        return new ResponseModel
        {
            Message = "Student Created Successfully",
            IsSuccess = true,
            Data = student,
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
            var roleId = await _unitOfWork.RoleRepository
                .SingleOrDefaultAsync(selector: x => x.Id, predicate: x => x.Name.Equals(RoleEnum.Student.ToString()));

            foreach (var studentImport in students.Data)
            {
                //var accountId = Guid.NewGuid();
                var student = _mapper.Map<Student>(studentImport);

                student.Id = Guid.NewGuid();

                student.Account = new Account
                {
                    Id = Guid.NewGuid(),
                    Email = studentImport.Email,
                    Phone = studentImport.Phone,
                    Password = studentImport.Phone,
                    RoleId = roleId,
                    Status = AccountStatus.Active,
                    CreateAt = DateTime.UtcNow
                };
                student.Account.Wallet = new Wallet
                {
                    Id = Guid.NewGuid(),
                    GoldBalance = 0,
                    AccountId = student.Account.Id,
                };              
                student.HighSchoolId = studentImportModel.highschoolId;
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
