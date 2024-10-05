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
            public const string HighSchoolsEndpoint = ApiEndpoint + "/high-schools";
            public const string HighSchoolEndpoint = ApiEndpoint + "/high-school/{id}";
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
            public const string StudentsEndpoint = ApiEndpoint + "/students";
            public const string StudentEndpoint = ApiEndpoint + "/student/{id}";
            public const string ImportStudentEndpoint = StudentsEndpoint + "/import";
        }
        public static class Wallet
        {
            public const string WalletsEndpoint = ApiEndpoint + "/wallet";
            public const string WalletEndpoint = ApiEndpoint + "/wallet/{id}";
            public const string WalletPutEndpoint = ApiEndpoint + "/wallet/distribution";
        }


    }
}
