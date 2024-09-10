using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Model.MBTI;
using Domain.Model;
using Domain.Entity;
using Domain.Model.Answer;

namespace Application.Interface.Service
{
    public interface IResultBMTITestService
    {
        Task<ResponseModel> CreateResultMBTITest(StudentSelectedModel result, List<AnswerModel> redis_Value);
        Task<IEnumerable<MBTITestModel>> GetMBTITestById(int id);
        Task<IEnumerable<AnswerModel>> GetListAnswerToRedis(int TestId);

    }
}
