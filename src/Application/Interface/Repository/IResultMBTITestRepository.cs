using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entity;
using Domain.Model;
using Domain.Model.Answer;
using Domain.Model.MBTI;

namespace Application.Interface.Repository
{
    public interface IResultMBTITestRepository : IGenericRepository<Result>
    {
        Task CreateStudentSelected(StudentSelectedModel result);
        Task<ResultModel> CalculateResultMBTITest(StudentSelectedModel result, List<Answer> redis_Value);
        Task<IEnumerable<MBTITestModel>> GetMBTITestById(int id);
        Task<IEnumerable<Answer>> GetListAnswerToRedis(int TestId);
    }
}
