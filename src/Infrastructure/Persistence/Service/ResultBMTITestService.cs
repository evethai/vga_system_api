using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Interface;
using Application.Interface.Service;
using AutoMapper;
using Domain.Entity;
using Domain.Model;
using Domain.Model.Answer;
using Domain.Model.MBTI;

namespace Infrastructure.Persistence.Service
{
    public class ResultBMTITestService : IResultBMTITestService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public ResultBMTITestService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ResponseModel> CreateResultMBTITest(StudentSelectedModel resultTest, List<AnswerModel> redis_Value)
        {
            //await _unitOfWork.ResultMBTITestRepository.CreateStudentSelected(resultTest);

            List<Answer> listAnswer = null;
            if (redis_Value != null)
            {
                 listAnswer = _mapper.Map<List<Answer>>(redis_Value);
            }

            var resultMBTI = await _unitOfWork.ResultMBTITestRepository.CalculateResultMBTITest(resultTest, listAnswer);
            var result = _mapper.Map<Result>(resultMBTI);
            //await _unitOfWork.ResultMBTITestRepository.AddAsync(result);
            //_unitOfWork.Save();

            return new ResponseModel
            {
                Message = "Success",
                IsSuccess = true,
                Data = resultMBTI.description
            };
        }

        public Task<IEnumerable<MBTITestModel>> GetMBTITestById(int id)
        {
            var result = _unitOfWork.ResultMBTITestRepository.GetMBTITestById(id);
            return result;
        }

        public async Task<IEnumerable<AnswerModel>> GetListAnswerToRedis(int TestId)
        {
            var listAnswer = await _unitOfWork.ResultMBTITestRepository.GetListAnswerToRedis(TestId);
            if(listAnswer == null)
            {
                throw new Exception("List answer not found");
            }
            var result = _mapper.Map<IEnumerable<AnswerModel>>(listAnswer);
            return result;
        }
    }
}
