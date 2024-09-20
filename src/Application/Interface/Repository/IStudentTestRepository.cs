using Domain.Entity;
using Domain.Enum;
using Domain.Model.PersonalGroup;
using Domain.Model.Question;
using Domain.Model.Test;

namespace Application.Interface.Repository
{
    public interface IStudentTestRepository : IGenericRepository<StudentTest>
    {

        Task<PersonalGroupModel> CalculateResultMBTITest(List<int> listAnswerId);
        Task<PersonalTestModel> GetTestById(int personalTestId);
        Task<PersonalGroupModel> CalculateResultHollandTest(List<int> listQuestionId);
        Task<IEnumerable<Question>> GetAllQuestionByTestId(int personalTestId);
        Task<IEnumerable<Answer>> GetAnswerByQuestionId(int questionId);
        Task<TestTypeCode> CheckTestType(int personalTestId);
        Task<IEnumerable<StudentTest?>> GetHistoryTestByStudentId(int studentId);
    }
}
