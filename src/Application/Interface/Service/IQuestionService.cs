﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Model.Question;
using Domain.Model.Response;

namespace Application.Interface.Service
{
    public interface IQuestionService
    {
        Task<ResponseQuestionModel> GetAllQuestionsByTestId(QuestionSearchModel model);
        Task<QuestionModel> GetQuestionById(int id);
        //Task<ResponseModel> CreateQuestion(QuestionPostModel questionModel);
        Task<ResponseModel> UpdateQuestion(QuestionPutModel questionModel, int id);
        Task<ResponseModel> CreateQuestionForPersonalTest(QuestionPostModel model);
        Task<ResponseModel> DeleteQuestion(int id);
    }
}
