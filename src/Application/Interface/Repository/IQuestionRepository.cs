using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entity;
using Domain.Model.Question;
using Domain.Model.Response;

namespace Application.Interface.Repository
{
    public interface IQuestionRepository : IGenericRepository<Question>
    {
        Task<ResponseModel> CreateQuestion(QuestionPostModel model);
        Task<ResponseModel> UpdateQuestion(QuestionPutModel model);
    }
}
