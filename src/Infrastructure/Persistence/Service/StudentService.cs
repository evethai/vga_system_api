﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Interface;
using Application.Interface.Service;
using AutoMapper;
using Domain.Entity;
using Domain.Model.Highschool;
using Domain.Model.Response;
using Domain.Model.Student;
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
        var student = await _unitOfWork.StudentRepository.GetByConditionAsync(filter, orderBy, pageIndex: searchModel.currentPage, pageSize: searchModel.pageSize);
        var total = await _unitOfWork.StudentRepository.CountAsync(filter);
        var listStudent = _mapper.Map<List<StudentModel>>(student);
        return new ResponseStudentModel
        {
            total = total,
            currentPage = searchModel.currentPage,
            students =listStudent,
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
        exitStudent.Email = putModel.Email;
        exitStudent.Phone = putModel.Phone;
        exitStudent.Status = putModel.Status;
        var result = await _unitOfWork.StudentRepository.UpdateAsync(exitStudent);
        _unitOfWork.Save();
        return new ResponseModel
        {
            Message = "Student Updated Successfully",
            IsSuccess = true,
            Data = result,
        };

    }
    public async Task<ResponseModel> CreateStudentAsyns(StudentPostModel postModel)
    {
        var student = _mapper.Map<Student>(postModel);
        var result = await _unitOfWork.StudentRepository.AddAsync(student);
        _unitOfWork.Save();
        return new ResponseModel
        {
            Message = " Student Created Successfully",
            IsSuccess = true,
            Data = student,
        };
    }

    public async Task<ResponseModel> DeleteStudent(Guid StudentId)
    {
        var student = await _unitOfWork.StudentRepository.GetByIdGuidAsync(StudentId);
        var result = await _unitOfWork.StudentRepository.DeleteAsync(student);
        _unitOfWork.Save();

        return new ResponseModel
        {
            Message = "Student Deleted Successfully",
            IsSuccess = true,
        };
    }

    #region Import Students From Json Async
    public async Task<ResponseModel> ImportStudentsFromJsonAsync(string stringJson, int highschoolId)
    {
        try
        {
            var students = JsonConvert.DeserializeObject<List<Student>>(stringJson);

            if (students == null || students.Count == 0)
            {
                return new ResponseModel
                {
                    IsSuccess = false,
                    Message = "No students to import."
                };
            }

            foreach (var student in students)
            {
                student.Id = Guid.NewGuid();
                student.HighSchoolId = highschoolId;

                await _unitOfWork.StudentRepository.AddAsync(student);
            }

            _unitOfWork.Save();

            return new ResponseModel
            {
                IsSuccess = true,
                Message = $"{students.Count} students imported successfully."
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
