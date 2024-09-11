using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Model.Highschool;
using Domain.Model.Response;
using Domain.Model.Student;

namespace Application.Interface.Service;
public interface IStudentService
{
    Task<ResponseStudentModel> GetListStudentAsync(StudentSearchModel searchModel);
    Task<StudentModel> GetStudentByIdAsync(Guid StudentId);
    Task<ResponseModel> CreateStudentAsyns(StudentPostModel postModel);
    Task<ResponseModel> UpdateStudentAsynsl(StudentPutModel putModel);
    Task<ResponseModel> DeleteStudent(Guid StudentId);
    Task<ResponseModel> ImportStudentsFromJsonAsync(string stringJson, int highschoolId);
}
