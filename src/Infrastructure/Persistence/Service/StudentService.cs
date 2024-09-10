using System;
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

    public async Task<ResponseModel> UpdateStudentAsynsl(StudentPutModel putModel)
    {
        var student = _mapper.Map<Student>(putModel);
        var result = await _unitOfWork.StudentRepository.UpdateAsync(student);
        _unitOfWork.Save();
        return new ResponseModel
        {
            Message = "Student Updated Successfully",
            IsSuccess = true,
            Data = student,
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
}
