using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Domain.Entity;
using Domain.Model.PersonalTest;
using Domain.Model.Question;
using Domain.Model.Response;

namespace Application.Interface.Repository
{
    public interface IQuestionRepository : IGenericRepository<Question>
    {
        Task<ResponseModel> CreateQuestion(QuestionPostModel model);
        Task<ResponseModel> UpdateQuestion(QuestionPutModel model, int id);
        Task<ResponseModel> AddHollandQuestions(Guid personalTestId, Guid testTypeId, List<DataQuestionHollandModel> questions);
        Task<ResponseModel> AddMbtiQuestions(Guid personalTestId, Guid testTypeId, List<DataQuestionMBTIModel> questions);

    }
}
