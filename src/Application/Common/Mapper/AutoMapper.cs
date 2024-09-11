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
using Domain.Entity;
using Domain.Model.Highschool;
using Domain.Model.Region;
using Domain.Model.Student;

namespace Application.Common.Mapper
{
    public class AutoMapper : Profile
    {
        public AutoMapper()
        {
            //Region
            CreateMap<Region, RegionModel>().ReverseMap();
            //MBTI Test
            CreateMap<Result, ResultModel>().ReverseMap();

            //Highschool
            CreateMap<HighSchool, HighschoolModel>().ReverseMap();
            CreateMap<HighSchool, HighschoolPostModel>().ReverseMap();
            CreateMap<HighSchool, HighschoolPutModel>().ReverseMap();

            //Student
            CreateMap<Student, StudentModel>().ReverseMap();
            CreateMap<Student, StudentPostModel>().ReverseMap();
            CreateMap<Student, StudentPutModel>().ReverseMap();
            CreateMap<Student, StudentJsonModel>().ReverseMap();
            //Answer
            CreateMap<Answer, AnswerModel>().ReverseMap();
        }
    }
}

