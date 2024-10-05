using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Entity;
using Domain.Enum;
using Domain.Model.AccountWallet;
using Domain.Model.Highschool;
using Domain.Model.Major;
using Domain.Model.PersonalGroup;
using Domain.Model.Question;
using Domain.Model.Region;
using Domain.Model.Student;
using Domain.Model.Test;
using Domain.Model.Transaction;
using Domain.Model.Wallet;

using Domain.Model.TestType;


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
            CreateMap<Student, StudentJsonModel>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender))
            .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.DateOfBirth))
            .ReverseMap();


            //Question
            CreateMap<Question, QuestionModel>().ReverseMap();
            CreateMap<Question, QuestionPostModel>().ReverseMap();

            //Answer
            CreateMap<Answer, AnswerModel>().ReverseMap();

            //Major
            CreateMap<Major, MajorModel>().ReverseMap();

            //PersonalGroup
            CreateMap<PersonalGroup, PersonalGroupModel>().ReverseMap();



            //Wallet
            CreateMap<Wallet, WalletModel>().ReverseMap();
            CreateMap<Wallet, WalletPutModel>().ReverseMap();
            
                
            //Transaction
            CreateMap<Transaction, TransactionModel>().ReverseMap();
            CreateMap<Transaction, TransactionPostModel>().ReverseMap();
            //TestType
            CreateMap<TestType, TestTypeModel>().ReverseMap();
            // Account
            CreateMap<Account, AccountWalletModel>().ReverseMap();
            CreateMap<Wallet, WalletAccountModel>().ReverseMap();
        }
    }
}

