using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Model.Response;
using Domain.Model.StudentChoice;

namespace Application.Interface.Service
{
    public interface IStudentChoiceService
    {
        Task<ResponseModel> CreateNewStudentCare(StudentChoicePostModel model);
        Task<ResponseModel> GetAllStudentCareById(Guid StudentId);
    }
}
