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
using Domain.Model.TimeSlot;
using Domain.Model.Transaction;
using Domain.Model.Wallet;

using Domain.Model.TestType;
using Domain.Model.Consultant;
using Domain.Model.ConsultationDay;
using Domain.Model.ConsultationTime;
using Domain.Model.Booking;
using Domain.Model.ConsultantLevel;
using Domain.Model.University;
using Domain.Model.Notification;
using Domain.Model.News;


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
            CreateMap<Student, StudentJsonModel>().ReverseMap();


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

            //Consultant level
            CreateMap<ConsultantLevel, ConsultantLevelViewModel>().ReverseMap();
            CreateMap<ConsultantLevel, ConsultantLevelPostModel>().ReverseMap();
            CreateMap<ConsultantLevel, ConsultantLevelPutModel>()
                .ReverseMap()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            //Time slot
            CreateMap<TimeSlot, TimeSlotViewModel>().ReverseMap();
            CreateMap<TimeSlot, TimeSlotPostModel>().ReverseMap();
            CreateMap<TimeSlot, TimeSlotPutModel>().ReverseMap();

            //Consultant
            CreateMap<Consultant, ConsultantViewModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.AccountId, opt => opt.MapFrom(src => src.AccountId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Account.Name))
                .ForMember(dest => dest.ConsultantLevel, opt => opt.MapFrom(src => src.ConsultantLevel))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Account.Email))
                .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.Account.Phone))
                .ForMember(dest => dest.Image_Url, opt => opt.MapFrom(src => src.Account.Image_Url))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.DoB))
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender))
                .ForMember(dest => dest.CreateAt, opt => opt.MapFrom(src => src.Account.CreateAt))
                .ReverseMap();
            CreateMap<Consultant, ConsultantPostModel>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Account.Name))
                .ForMember(dest => dest.ConsultantLevelId, opt => opt.MapFrom(src => src.ConsultantLevelId))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.DoB, opt => opt.MapFrom(src => src.DoB))
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender))
                .ReverseMap();
            CreateMap<Consultant, ConsultantPutModel>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Account.Name))
                .ForMember(dest => dest.ConsultantLevelId, opt => opt.MapFrom(src => src.ConsultantLevelId))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Account.Email))
                .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.Account.Phone))
                .ForMember(dest => dest.Image_Url, opt => opt.MapFrom(src => src.Account.Image_Url))
                .ReverseMap();

            //Consultation Day
            CreateMap<ConsultationDay, ConsultationDayViewModel>()
                .ReverseMap();
            CreateMap<ConsultationDay, ConsultationDayPostModel>()
               .ReverseMap();

            //Consultation Time
            CreateMap<ConsultationTime, ConsultationTimeViewModel>()
                .ForMember(dest => dest.StartTime, opt => opt.MapFrom(src => src.SlotTime.StartTime))
                .ForMember(dest => dest.EndTime, opt => opt.MapFrom(src => src.SlotTime.EndTime))
                .ReverseMap();
            CreateMap<ConsultationTime, ConsultationTimePostModel>()
                .ReverseMap();

            //Booking
            CreateMap<Booking, BookingViewModel>()
                .ForMember(dest => dest.ConsultationTimeId, opt => opt.MapFrom(src => src.ConsultationTime.Id))
                .ForMember(dest => dest.TimeSlotId, opt => opt.MapFrom(src => src.ConsultationTime.TimeSlotId))
                .ForMember(dest => dest.StartTime, opt => opt.MapFrom(src => src.ConsultationTime.SlotTime.StartTime))
                .ForMember(dest => dest.EndTime, opt => opt.MapFrom(src => src.ConsultationTime.SlotTime.EndTime))
                .ForMember(dest => dest.ConsultationDayId, opt => opt.MapFrom(src => src.ConsultationTime.ConsultationDayId))
                .ForMember(dest => dest.ConsultationDay, opt => opt.MapFrom(src => src.ConsultationTime.Day.Day))
                .ForMember(dest => dest.ConsultantId, opt => opt.MapFrom(src => src.ConsultationTime.Day.Consultant.Id))
                .ForMember(dest => dest.ConsultantName, opt => opt.MapFrom(src => src.ConsultationTime.Day.Consultant.Account.Name))
                .ForMember(dest => dest.ConsultantEmail, opt => opt.MapFrom(src => src.ConsultationTime.Day.Consultant.Account.Email))
                .ForMember(dest => dest.ConsultantPhone, opt => opt.MapFrom(src => src.ConsultationTime.Day.Consultant.Account.Phone))
                .ForMember(dest => dest.StudentId, opt => opt.MapFrom(src => src.Student.Id))
                .ForMember(dest => dest.StudentName, opt => opt.MapFrom(src => src.Student.Account.Name))
                .ForMember(dest => dest.StudentEmail, opt => opt.MapFrom(src => src.Student.Account.Email))
                .ForMember(dest => dest.StudentPhone, opt => opt.MapFrom(src => src.Student.Account.Phone))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status));

            // Account
            CreateMap<Account, AccountWalletModel>().ReverseMap();
            CreateMap<Wallet, WalletAccountModel>().ReverseMap();
            // University
            CreateMap<University, UniversityModel>().ReverseMap();
            CreateMap<University, UniversityPostModel>().ReverseMap();
            CreateMap<University, UniversityPutModel>().ReverseMap();

            //Notification
            CreateMap<Notification, NotificationModel>().ReverseMap();
            CreateMap<Notification, NotificationPostModel>().ReverseMap();

            //StudentChoice
            CreateMap<Major, MajorOrOccupationModel>().ReverseMap();
        }
      
    }
}

