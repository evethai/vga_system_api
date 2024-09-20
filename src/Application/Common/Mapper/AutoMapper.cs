using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Entity;
using Domain.Model.Highschool;
using Domain.Model.Major;
using Domain.Model.PersonalGroup;
using Domain.Model.Question;
using Domain.Model.Region;
using Domain.Model.Student;
using Domain.Model.Test;

namespace Application.Common.Mapper
{
    public class AutoMapper : Profile
    {
        public AutoMapper()
        {
            //Region
            CreateMap<Region, RegionModel>().ReverseMap();

            //PersonalTest
            CreateMap<PersonalTest, PersonalTestModel>().ReverseMap();
            CreateMap<StudentTest, StudentTestModel>().ReverseMap();

            //StudentTest
            CreateMap<StudentTest, StudentTestModel>().ReverseMap();
            CreateMap<StudentTest, HistoryTestModel>().ReverseMap();


            //Highschool
            CreateMap<HighSchool, HighschoolModel>().ReverseMap();
            CreateMap<HighSchool, HighschoolPostModel>().ReverseMap();
            CreateMap<HighSchool, HighschoolPutModel>().ReverseMap();

            //Student
            CreateMap<Student, StudentModel>().ReverseMap();
            CreateMap<Student, StudentPostModel>().ReverseMap();
            CreateMap<Student, StudentPutModel>().ReverseMap();

            //Question
            CreateMap<Question, QuestionModel>().ReverseMap();

            //Answer
            CreateMap<Answer, AnswerModel>().ReverseMap();

            //Major
            CreateMap<Major, MajorModel>().ReverseMap();

            //PersonalGroup
            CreateMap<PersonalGroup, PersonalGroupModel>().ReverseMap();

        }
    }
}

