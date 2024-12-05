using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Interface;
using Application.Interface.Service;
using AutoMapper;
using Domain.Entity;
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
        public async Task<ResponseQuestionModel> GetAllQuestionsByTestId(QuestionSearchModel model)
        {
            //var questions = await _unitOfWork.TestQuestionRepository.GetListAsync(predicate: x=>x.PersonalTestId == model.PersonalTestId && x.Question.Status == true,include: x => x.Include(q => q.Question).ThenInclude(a => a.Answers));
            var conditions = _unitOfWork.TestQuestionRepository.BuildFilterAndOrderBy(model);
            var questions = await _unitOfWork.TestQuestionRepository.GetByConditionAsync(conditions.filter, conditions.orderBy, includeProperties: "Question,Question.Answers", pageIndex: model.Page, pageSize: model.Size);

            var result = _mapper.Map<IEnumerable<QuestionListByTestIdModel>>(questions);
            var total = await _unitOfWork.TestQuestionRepository.CountAsync(conditions.filter);
            var response = new ResponseQuestionModel
            {
                total = total,
                currentPage = model.Page,
                questions = result.ToList()
            };
            return response;
        }
        public async Task<QuestionModel> GetQuestionById(int id)
        {
            var question = await _unitOfWork.QuestionRepository.SingleOrDefaultAsync(predicate: x => x.Id == id, include: x=>x.Include(question => question.Answers));
            var result = _mapper.Map<QuestionModel>(question);
            return result;
        }

        public async Task<ResponseModel> UpdateQuestion(QuestionPutModel questionModel, int id)
        {
            var result = await _unitOfWork.QuestionRepository.UpdateQuestion(questionModel, id);
            return result;
        }

        public async Task<ResponseModel> CreateQuestionForPersonalTest(QuestionPostModel model)
        {
            var result = await _unitOfWork.QuestionRepository.CreateQuestion(model);
            return result;
        }

        public async Task<ResponseModel> DeleteQuestion(int id)
        {
            var question = await _unitOfWork.QuestionRepository.SingleOrDefaultAsync(predicate: x => x.Id == id);
            if (question == null)
            {
                return new ResponseModel
                {
                    Message = "Question not found",
                    IsSuccess = false
                };
            }
            question.Status = false;
            await _unitOfWork.QuestionRepository.UpdateAsync(question);
            await _unitOfWork.SaveChangesAsync();
            return new ResponseModel
            {
                Message = "Question deleted successfully",
                IsSuccess = true
            };
        }
    }
}
