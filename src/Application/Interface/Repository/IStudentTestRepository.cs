﻿using Domain.Entity;
using Domain.Enum;
using Domain.Model.PersonalGroup;
using Domain.Model.Question;
using Domain.Model.Test;

namespace Application.Interface.Repository
{
    public interface IStudentTestRepository : IGenericRepository<StudentTest>
    {

        Task<PersonalGroupModel> CalculateResultMBTITest(List<int> listAnswerId);
        Task<PersonalTestModel> GetTestById(Guid personalTestId);
        Task<PersonalGroupModel> CalculateResultHollandTest(List<int> listQuestionId);
        Task<TestTypeCode> CheckTestType(Guid personalTestId);
        Task<IEnumerable<HistoryTestModel>> GetHistoryTestByStudentId(Guid studentId);
        Task<IEnumerable<Major>> GetMajorsByPersonalGroupId(Guid personalGroupId);
        Task<IEnumerable<Occupation>> GetOccupationByMajorId(Guid majorId);
        Task<List<Guid>> CreateStudentChoice(StudentChoiceModel StModel, StudentChoiceType type);
    }
}
