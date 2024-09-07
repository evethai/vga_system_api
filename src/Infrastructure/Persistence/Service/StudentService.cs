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
    public Task<ResponseStudentModel> GetListStudentAsync(StudentSearchModel searchModel)
    {
        throw new NotImplementedException();
    }

    public async Task<StudentModel> GetStudentByIdAsync(Guid StudentId)
    {
        var student = await _unitOfWork.StudentRepository.GetByIdGuidAsync(StudentId);
        return _mapper.Map<StudentModel>(student);
    }

    public Task<ResponseModel> UpdateStudentAsynsl(StudentPutModel putModel)
    {
        throw new NotImplementedException();

    }
    public Task<ResponseModel> CreateStudentAsyns(StudentPostModel postModel)
    {
        throw new NotImplementedException();
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
