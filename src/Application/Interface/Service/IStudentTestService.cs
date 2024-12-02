using Domain.Entity;
using Domain.Model;
using Domain.Model.PersonalTest;
using Domain.Model.Question;
using Domain.Model.Response;
using Domain.Model.Test;


namespace Application.Interface.Service
{
    public interface IStudentTestService
    {

        Task<ResponseModel> CreateResultForTest(StudentTestResultModel result);
        Task<PersonalTestModel> GetTestById(Guid id);
        Task<ResponsePersonalTestModel> GetAllTest(PersonalTestSearchModel model);
        Task<StudentHistoryModel> GetHistoryTestByStudentId(Guid studentId);
        Task<ResponseModel> GetMajorsByPersonalGroupId(Guid stTestId);
        Task<ResponseModel> FilterOccupationAndUniversity(FilterMajorAndUniversityModel model);

    }
}
