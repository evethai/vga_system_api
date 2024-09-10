using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Entity;
using Domain.Model.Answer;
using Domain.Model.MBTI;

namespace Application.Common.Mapper
{
    public class AutoMapper : Profile
    {
        public AutoMapper()
        {
            //MBTI Test
            CreateMap<Result,ResultModel>().ReverseMap();

            //Answer
            CreateMap<Answer, AnswerModel>().ReverseMap();
        }
    }
}

