using AutoMapper;
using Domain.Entity;
using Domain.Model.AccountWallet;
using Domain.Model.AdmissionInformation;
using Domain.Model.Booking;
using Domain.Model.Consultant;
using Domain.Model.ConsultantLevel;
using Domain.Model.ConsultationDay;
using Domain.Model.ConsultationTime;
using Domain.Model.EntryLevelEducation;
using Domain.Model.Highschool;
using Domain.Model.Major;
using Domain.Model.MajorCategory;
using Domain.Model.News;
using Domain.Model.Notification;
using Domain.Model.Occupation;
using Domain.Model.OccupationalGroup;
using Domain.Model.OccupationalSkills;
using Domain.Model.PersonalGroup;
using Domain.Model.Question;
using Domain.Model.Region;
using Domain.Model.Student;
using Domain.Model.Test;
using Domain.Model.TestType;
using Domain.Model.TimeSlot;
using Domain.Model.Transaction;
using Domain.Model.University;
using Domain.Model.Wallet;
using Domain.Model.WorkSkills;



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
                .ForMember(dest => dest.University, opt => opt.MapFrom(src => src.University))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Account.Email))
                .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.Account.Phone))
                .ForMember(dest => dest.Image_Url, opt => opt.MapFrom(src => src.Account.Image_Url))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.DoB))
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender))
                .ForMember(dest => dest.CreateAt, opt => opt.MapFrom(src => src.Account.CreateAt))
                .ReverseMap();
            CreateMap<Consultant, ConsultantPostModel>()
                //.ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Account.Name))
                //.ForMember(dest => dest.ConsultantLevelId, opt => opt.MapFrom(src => src.ConsultantLevelId))
                //.ForMember(dest => dest.UniversityId, opt => opt.MapFrom(src => src.UniversityId))
                //.ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                //.ForMember(dest => dest.DoB, opt => opt.MapFrom(src => src.DoB))
                //.ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender))
                .ReverseMap();
            CreateMap<Consultant, ConsultantPutModel>()
                //.ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Account.Name))
                //.ForMember(dest => dest.ConsultantLevelId, opt => opt.MapFrom(src => src.ConsultantLevelId))
                //.ForMember(dest => dest.UniversityId, opt => opt.MapFrom(src => src.UniversityId))
                //.ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                //.ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Account.Email))
                //.ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.Account.Phone))
                //.ForMember(dest => dest.Image_Url, opt => opt.MapFrom(src => src.Account.Image_Url))
                .ReverseMap();

            //Consultation Day
            CreateMap<ConsultationDay, ConsultationDayViewModel>()
                .ReverseMap();
            CreateMap<Consultant, ConsultantModel>()
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
            CreateMap<University, UniversityModelGetBy>().ReverseMap();
            CreateMap<University, UniversityPostModel>().ReverseMap();
            CreateMap<University, UniversityPutModel>().ReverseMap();
            // University
            CreateMap<UniversityLocation, UniversityLocationModel>().ReverseMap();            
            CreateMap<UniversityLocation, UniversityLocationPutModel>().ReverseMap();

            //Notification
            CreateMap<Notification, NotificationModel>().ReverseMap();
            CreateMap<Notification, NotificationPostModel>().ReverseMap();

            //StudentChoice
            CreateMap<Major, MajorOrOccupationModel>().ReverseMap();

            //EntryLevelEducation
            CreateMap<EntryLevelEducation, EntryLevelEducationViewModel>().ReverseMap();
            CreateMap<EntryLevelEducation, EntryLevelEducationPostModel>().ReverseMap();
            CreateMap<EntryLevelEducation, EntryLevelEducationPutModel>().ReverseMap();

            //Major
            CreateMap<Major, MajorViewModel>()
                .ForMember(dest => dest.MajorCategoryName, opt => opt.MapFrom(src => src.MajorCategory.Name))
                .ReverseMap();
            CreateMap<Major, MajorPostModel>().ReverseMap();
            CreateMap<Major, MajorPutModel>().ReverseMap();

            CreateMap<OccupationByMajorIdModel, Occupation>().ReverseMap();
            CreateMap<UniversityByMajorIdModel, University>().ReverseMap();

            //MajorCategory
            CreateMap<MajorCategory, MajorCategoryViewModel>().ReverseMap();
            CreateMap<MajorCategory, MajorCategoryPostModel>().ReverseMap();
            CreateMap<MajorCategory, MajorCategoryPutModel>().ReverseMap();

            //Occupation
            CreateMap<Occupation, OccupationViewModel>()
                .ForMember(dest => dest.EntryLevelEducation, opt => opt.MapFrom(src => src.EntryLevelEducation))
                .ForMember(dest => dest.OccupationalGroup, opt => opt.MapFrom(src => src.OccupationalGroup))
                .ForMember(dest => dest.OccupationalSkills, opt => opt.MapFrom(src => src.OccupationalSKills))
                .ReverseMap();
            CreateMap<Occupation, OccupationPostModel>().ReverseMap();
            CreateMap<Occupation, OccupationPutModel>()
                .ReverseMap()
                .ForMember(dest => dest.OccupationalSKills, opt => opt.Ignore());

            //OccupationalGroup
            CreateMap<OccupationalGroup, OccupationalGroupViewModel>().ReverseMap();
            CreateMap<OccupationalGroup, OccupationalGroupPostModel>().ReverseMap();
            CreateMap<OccupationalGroup, OccupationalGroupPutModel>().ReverseMap();

            //OccupationSkill
            CreateMap<OccupationalSKills, OccupationalSkillsViewModel>().ReverseMap();
            CreateMap<OccupationalSKills, OccupationalSkillsPostModel>().ReverseMap();
            CreateMap<OccupationalSKills, OccupationalSkillsPutModel>().ReverseMap();

            //WorkSkill
            CreateMap<WorkSkills, WorkSkillsViewModel>().ReverseMap();
            CreateMap<WorkSkills, WorkSkillsPostModel>().ReverseMap();
            CreateMap<WorkSkills, WorkSkillsPutModel>().ReverseMap();
            //AdmissionInformation
            CreateMap<AdmissionInformation, AdmissionInformationModel>()
                .ForMember(dest => dest.UniversityName, opt => opt.MapFrom(src => src.University.Account.Name))
                .ForMember(dest => dest.AdmissionMethodName, opt => opt.MapFrom(src => src.AdmissionMethod.Name))
                .ForMember(dest => dest.MajorName, opt => opt.MapFrom(src => src.Major.Name))
                .ReverseMap();
            CreateMap<AdmissionInformation, AdmissionInformationPostModel>().ReverseMap();
            CreateMap<AdmissionInformation, AdmissionInformationPutModel>().ReverseMap();
            //AdmissionMethod
            CreateMap<AdmissionMethod, AdmissionMethodModel>().ReverseMap();
            CreateMap<AdmissionMethod, AdmissionMethodPostModel>().ReverseMap();
            CreateMap<AdmissionMethod, AdmissionMethodPutModel>().ReverseMap();
            //News
            CreateMap<News, NewsModel>().ReverseMap();
            CreateMap<News, NewsPostModel>().ReverseMap();
            CreateMap<News, NewsPutModel>().ReverseMap();
            //NewsImage
            CreateMap<ImageNews, ImageNewsModel>().ReverseMap();
            CreateMap<ImageNews, ImageNewsPostModel>().ReverseMap();
            CreateMap<ImageNews, ImageNewsPutModel>().ReverseMap();

        }

    }
}

