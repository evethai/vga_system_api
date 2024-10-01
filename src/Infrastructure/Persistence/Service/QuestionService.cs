using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Interface;
using Application.Interface.Service;
using AutoMapper;
using Domain.Model.Question;
using Domain.Model.Response;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Service
{
    public class QuestionService: IQuestionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public QuestionService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<IEnumerable<QuestionModel>> GetAllQuestionsByType(Guid id)
        {
            var questions = await _unitOfWork.QuestionRepository.GetListAsync(predicate: x=>x.TestTypeId == id,include: x => x.Include(questions=>questions.Answers));
            var result = _mapper.Map<IEnumerable<QuestionModel>>(questions);
            return result;
        }
        public async Task<QuestionModel> GetQuestionById(int id)
        {
            var question = await _unitOfWork.QuestionRepository.SingleOrDefaultAsync(predicate: x => x.Id == id, include: x=>x.Include(question => question.Answers));
            var result = _mapper.Map<QuestionModel>(question);
            return result;
        }


        public async Task<ResponseModel> CreateQuestion(QuestionPostModel questionModel)
        {
            var result = await _unitOfWork.QuestionRepository.CreateQuestion(questionModel);
            return result;
        }

        public async Task<ResponseModel> UpdateQuestion(QuestionPutModel questionModel)
        {
            var result = await _unitOfWork.QuestionRepository.UpdateQuestion(questionModel);
            return result;
        }
    }
}
