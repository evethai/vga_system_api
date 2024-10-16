﻿namespace Api.Constants
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
        public static class Wallet
        {
            public const string WalletsEndpoint = ApiEndpoint + "/wallet";
            public const string WalletEndpoint = ApiEndpoint + "/wallet/{id}";
            public const string WalletDistribution = ApiEndpoint + "/wallet/distribution";
            public const string WalletTest = ApiEndpoint + "/wallet/do-the-test";
            public const string WalletBook = ApiEndpoint + "/wallet/book-conslutant";
        }


  
        //consultant
        public static class Consultant
        {
            public const string ConsultantsEndpoint = ApiEndpoint + "/consultants";
            public const string ConsultantEndpoint = ConsultantsEndpoint + "/{id}";
        }

        //time slot
        public static class TimeSlot
        {
            public const string TimeSlotsEndpoint = ApiEndpoint + "/timeslots";
            public const string TimeSlotEndpoint = TimeSlotsEndpoint + "/{id}";
            public const string TimeSlotsGetAllEndpoint = TimeSlotsEndpoint + "/all";
        }

        //consultant level
        public static class ConsultantLevel
        {
            public const string ConsultantLevelsEndpoint = ApiEndpoint + "/consultant-levels";
            public const string ConsultantLevelEndpoint = ConsultantLevelsEndpoint + "/{id}";
        }

        //consultation day
        public static class ConsultationDay
        {
            public const string ConsultationDaysEndpoint = ApiEndpoint + "/consultation-days";
            public const string ConsultationDayEndpoint = ConsultationDaysEndpoint + "/{id}";
        }

        //consultation time
        public static class ConsultationTime
        {
            public const string ConsultationTimesEndpoint = ApiEndpoint + "/consultation-times";
            public const string ConsultationTimeEndpoint = ConsultationTimesEndpoint + "/{id}";
        }

        //Booking
        public static class Booking
        {
            public const string BookingsEndpoint = ApiEndpoint + "/bookings";
            public const string BookingEndpoint = BookingsEndpoint + "/{id}";
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

        //Notification
        public static class Notification
        {
            public const string NotificationsEndpoint = ApiEndpoint + "/notifications";
            public const string NotificationEndpoint = ApiEndpoint + "/notification/account/{id}";
        }
    }
}
