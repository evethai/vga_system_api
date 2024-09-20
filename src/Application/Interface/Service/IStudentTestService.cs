using Domain.Entity;
using Domain.Model;
using Domain.Model.Question;
using Domain.Model.Response;
using Domain.Model.Test;


namespace Application.Interface.Service
{
    public interface IStudentTestService
    {

        Task<ResponseModel> CreateResultForTest(StudentTestResultModel result);
        Task<PersonalTestModel> GetTestById(int id);
        Task<IEnumerable<PersonalTestModel>> GetAllTest();
        Task<IEnumerable<QuestionModel>> GetQuestionByTestId(int id);
        Task<IEnumerable<AnswerModel>> GetAnswerByQuestionId(int id);
        Task<IEnumerable<HistoryTestModel?>> GetHistoryTestByStudentId(int studentId);

    }
}
