namespace Api.Constants
{
    public static class ApiEndPointConstant
    {
        static ApiEndPointConstant()
        {
        }

        public const string RootEndPoint = "/api";
        public const string ApiVersion = "/v1";
        public const string ApiEndpoint = RootEndPoint + ApiVersion;

        public static class Account
        {
            public const string AccountsEndpoint = ApiEndpoint + "/accounts";
            public const string AccountEndpoint = AccountsEndpoint + "/{id}";
            public const string LoginEndpoint = ApiEndpoint + "/login";
            public const string LoginZaloEndpoint = ApiEndpoint + "/login-zalo";
            public const string RefreshTokenEndpoint = ApiEndpoint + "/refresh-token";
            public const string LogoutEndpoint = ApiEndpoint + "/logout/{id}";
        }

        public static class PersonalTest
        {
            public const string PersonalTestsEndpoint = ApiEndpoint + "/personal-tests";
            public const string PersonalTestEndpoint = ApiEndpoint + "/personal-test/{id}";
            public const string GetHistoryUserTestEndpoint = ApiEndpoint + "/history-user/{id}";
            public const string GetResultPersonalTestEndpoint = ApiEndpoint + "/personal-test-result";
            public const string GetMajorsByPersonalGroupIdEndpoint = ApiEndpoint + "/majors-by-personality/{id}";
            public const string FilterMajorAndUniversityEndpoint = ApiEndpoint + "/filter-major-university";
        }

        public static class PersonalGroup
        {
            public const string PersonalGroupsEndpoint = ApiEndpoint + "/personal-groups";
            public const string PersonalGroupEndpoint = ApiEndpoint + "/personal-group/{id}";
        }

        public static class Question
        {
            public const string QuestionsEndpoint = ApiEndpoint + "/questions";
            public const string QuestionEndpoint = ApiEndpoint + "/question/{id}";
        }

        public static class TestType
        {
            public const string TestTypesEndpoint = ApiEndpoint + "/test-types";
            public const string TestTypeEndpoint = ApiEndpoint + "/test-type/{id}";
        }

        //high-school
        public static class HighSchool
        {
            public const string HighSchoolGetListEndpoint = ApiEndpoint + "/high-schools";
            public const string HighSchoolEndpoint = ApiEndpoint + "/high-school/{id}";
            public const string HighSchoolPostEndpoint = ApiEndpoint + "/high-school";
            public const string HighSchoolPutEndpoint = ApiEndpoint + "/high-school/{id}";
            public const string HighSchoolDeleteEndpoint = ApiEndpoint + "/high-school/{id}";
        }

        //region
        public static class Region
        {
            public const string RegionsEndpoint = ApiEndpoint + "/regions";
            public const string RegionEndpoint = ApiEndpoint + "/region/{id}";
        }

        //student
        public static class Student
        {
            public const string StudentGetListEndpoint = ApiEndpoint + "/students";
            public const string StudentPostEndpoint = ApiEndpoint + "/student";
            public const string StudentPutEndpoint = ApiEndpoint + "/student/{id}";
            public const string StudentEndpoint = ApiEndpoint + "/student/{id}";
            public const string StudentDeleteEndpoint = ApiEndpoint + "/student/{id}";
            public const string ImportStudentEndpoint = StudentGetListEndpoint + "/import";
        }
        //Wallet
        public static class Wallet
        {
            public const string WalletsEndpoint = ApiEndpoint + "/wallet";
            public const string WalletEndpoint = ApiEndpoint + "/wallet/{id}";
            public const string WalletDistribution = ApiEndpoint + "/wallet/distribution";
            public const string WalletTest = ApiEndpoint + "/wallet/do-the-test";
            public const string WalletTransferringAndReceiving = ApiEndpoint + "/wallet/tranferring-gold";
        }
        //Transaction
        public static class Transaction
        {
            public const string TransactionEndPoint = ApiEndpoint + "/transactions";
        }

        //consultant
        public static class Consultant
        {
            public const string ConsultantsEndpoint = ApiEndpoint + "/consultants";
            public const string ConsultantEndpoint = ApiEndpoint + "/consultant/{id}";
        }

        //time slot
        public static class TimeSlot
        {
            public const string TimeSlotsEndpoint = ApiEndpoint + "/timeslots";
            public const string TimeSlotEndpoint = ApiEndpoint + "/timeslot/{id}";
            public const string TimeSlotsGetAllEndpoint = TimeSlotsEndpoint + "/all";
        }

        //consultant level
        public static class ConsultantLevel
        {
            public const string ConsultantLevelsEndpoint = ApiEndpoint + "/consultant-levels";
            public const string ConsultantLevelEndpoint = ApiEndpoint + "/consultant-level/{id}";
        }

        //consultation day
        public static class ConsultationDay
        {
            public const string ConsultationDaysEndpoint = ApiEndpoint + "/consultation-days";
            public const string ConsultationDayEndpoint = ApiEndpoint + "/consultation-day/{id}";
        }

        //consultation time
        public static class ConsultationTime
        {
            public const string ConsultationTimesEndpoint = ApiEndpoint + "/consultation-times";
            public const string ConsultationTimeEndpoint = ApiEndpoint + "/consultation-time/{id}";
        }

        //Booking
        public static class Booking
        {
            public const string BookingsEndpoint = ApiEndpoint + "/bookings";
            public const string BookingEndpoint = ApiEndpoint + "/booking/{id}";
        }
        //University
        public static class University
        {
            public const string UniversitiesEndpoint = ApiEndpoint + "/universities";
            public const string UniversityEndpoint = ApiEndpoint + "/university/{id}";
            public const string UniversityPutEndpoint = ApiEndpoint + "/university/{id}";
            public const string UniversityDeleteEndpoint = ApiEndpoint + "/university/{id}";
            public const string UniversityPostEndpoint = ApiEndpoint + "/university";
        }
        //University
        public static class UniversityLocation
        {
            public const string UniversityLocationPutEndpoint = ApiEndpoint + "/university-location/{id}";
            public const string UniversityLocationDeleteEndpoint = ApiEndpoint + "/university-location/{id}";
            public const string UniversityLocationPostEndpoint = ApiEndpoint + "/university-location/{UniversityId}";
        }


        //Notification
        public static class Notification
        {
            public const string NotificationsEndpoint = ApiEndpoint + "/notifications";
            public const string NotificationEndpoint = ApiEndpoint + "/notification/account/{id}";
        }
        //News
        public static class News
        {
            public const string NewsEndpoint = ApiEndpoint + "/news";
            public const string NewEndpoint = ApiEndpoint + "/news/{id}";
        }
        // Image News
        public static class ImageNews
        {
            public const string ImageNewsEndpoint = ApiEndpoint + "/image-news";
            public const string ImageNewsDeleteEndpoint = ApiEndpoint + "/image-news/{id}";
            public const string ImageNewsPutEndpoint = ApiEndpoint + "/image-news/{id}";
        }
        public static class AdmisstionInformation
        {
            public const string AdmisstionInformationListEndpoint = ApiEndpoint + "/admission-informations";
            public const string AdmisstionInformationEndpoint = ApiEndpoint + "/admission-information/{id}";
            public const string AdmisstionInformationPostEndpoint = ApiEndpoint + "/admission-information/{UniversityId}";
            public const string AdmisstionInformationPutEndpoint = ApiEndpoint + "/admission-informations/{id}";
            public const string AdmisstionInformationDeleteEndpoint = ApiEndpoint + "/admission-information/{id}";
        }
        public static class AdmisstionMethod
        {
            public const string AdmisstionMethodListEndpoint = ApiEndpoint + "/admission-methods";
            public const string AdmisstionMethodEndpoint = ApiEndpoint + "/admission-method/{id}";
            public const string AdmisstionMethodPostEndpoint = ApiEndpoint + "/admission-method";
            public const string AdmisstionMethodPutEndpoint = ApiEndpoint + "/admission-method/{id}";
            public const string AdmisstionMethodDeleteEndpoint = ApiEndpoint + "/admission-method/{id}";
        }

        //EntryLevelEducation
        public static class EntryLevelEducation
        {
            public const string EntryLevelEducationsEndpoint = ApiEndpoint + "/entry-level-educations";
            public const string EntryLevelEducationEndpoint = ApiEndpoint + "/entry-level-education/{id}";
        }

        //Major
        public static class Major
        {
            public const string MajorsEndpoint = ApiEndpoint + "/majors";
            public const string MajorEndpoint = ApiEndpoint + "/major/{id}";
            public const string MajorAndRelationEndpoint = ApiEndpoint + "/major-and-relation/{id}";
        }

        //MajorCategory
        public static class MajorCategory
        {
            public const string MajorCategoriesEndpoint = ApiEndpoint + "/major-categories";
            public const string MajorCategoryEndpoint = ApiEndpoint + "/major-category/{id}";
        }

        //Occupation
        public static class Occupation
        {
            public const string OccupationsEndpoint = ApiEndpoint + "/occupations";
            public const string OccupationEndpoint = ApiEndpoint + "/occupation/{id}";
        }

        //OccupationalGroup
        public static class OccupationalGroup
        {
            public const string OccupationalGroupsEndpoint = ApiEndpoint + "/occupational-groups";
            public const string OccupationalGroupEndpoint = ApiEndpoint + "/occupational-group/{id}";
        }

        //WorkSkill
        public static class WorkSkill
        {
            public const string WorkSkillsEndpoint = ApiEndpoint + "/work-skills";
            public const string WorkSkillEndpoint = ApiEndpoint + "/work-skill/{id}";
        }
    }
}
