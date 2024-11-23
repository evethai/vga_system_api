using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Interface;
using Application.Interface.Repository;
using AutoMapper;
using Domain.Entity;
using Domain.Enum;
using Domain.Model.Major;
using Domain.Model.PersonalGroup;
using Domain.Model.Question;
using Domain.Model.Test;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Infrastructure.Persistence.Repository;

public class StudentTestRepository : GenericRepository<StudentTest>, IStudentTestRepository
{
    private readonly VgaDbContext _context;
    public StudentTestRepository(VgaDbContext context) : base(context)
    {
        _context = context;
    }
    #region MBTI Test
    public async Task<PersonalGroupModel> CalculateResultMBTITest(List<int> listAnswerId)
    {
        var answerValueCounts = new Dictionary<AnswerValue, int>
        {
            { AnswerValue.E, 0 },
            { AnswerValue.I, 0 },
            { AnswerValue.S, 0 },
            { AnswerValue.N, 0 },
            { AnswerValue.T, 0 },
            { AnswerValue.F, 0 },
            { AnswerValue.J, 0 },
            { AnswerValue.P, 0 }
        };

        var answers = await _context.Answer
            .Where(a => listAnswerId.Contains(a.Id))
            .ToListAsync();

        foreach (var answer in answers)
        {
            answerValueCounts[answer.AnswerValue]++;
        }

        var mbtiType = string.Empty;
        mbtiType += (answerValueCounts[AnswerValue.I] >= answerValueCounts[AnswerValue.E]) ? "I" : "E";
        mbtiType += (answerValueCounts[AnswerValue.N] >= answerValueCounts[AnswerValue.S]) ? "N" : "S";
        mbtiType += (answerValueCounts[AnswerValue.F] >= answerValueCounts[AnswerValue.T]) ? "F" : "T";
        mbtiType += (answerValueCounts[AnswerValue.P] >= answerValueCounts[AnswerValue.J]) ? "P" : "J";


        var m_Type = await _context.PersonalGroup
            .Where(p => p.Code == mbtiType)
            .FirstOrDefaultAsync();

        if (m_Type == null)
        {
            throw new KeyNotFoundException("Personality type not found");
        }
        var result = new PersonalGroupModel
        {
            Id = m_Type.Id,
            Code = m_Type.Code,
            Description = m_Type.Description,
            Name = m_Type.Name

        };

        return result;
    }
    #endregion

    #region Get Test
    public async Task<PersonalTestModel> GetTestById(Guid personalTestId)
    {

        var test = await _context.PersonalTest
            .Where(p => p.Id == personalTestId)
            .Include(p => p.TestType)
            .Select(p => new
            {
                p.Id,
                p.TestTypeId,
                p.Name,
                p.Description,
                p.TestType.Point,
                Questions = p.TestQuestions
                .Where(q => q.Status == true)
                .Select(tq => new
                {
                    tq.Question.Id,
                    tq.Question.Content,
                    tq.Question.Group,
                    tq.Question.TestTypeId,
                    Answers = tq.Question.Answers.Select(a => new AnswerModel
                    {
                        Id = a.Id,
                        Content = a.Content,
                        AnswerValue = a.AnswerValue,
                    }).ToList()
                }).ToList()
            })
            .FirstOrDefaultAsync();

        if (test == null)
        {
            throw new KeyNotFoundException("Test not found");
        }

        var model = new PersonalTestModel
        {
            Id = test.Id,
            TestTypeId = test.TestTypeId,
            Name = test.Name,
            Description = test.Description,
            Point = test.Point,
            QuestionModels = test.Questions.Select(q => new QuestionModel
            {
                Id = q.Id,
                Content = q.Content,
                Group = q.Group,
                AnswerModels = q.Answers
            }).OrderBy(q => q.Group).ToList()
        };

        return model;
    }
    #endregion

    #region Holland Test
    public async Task<PersonalGroupModel> CalculateResultHollandTest(List<int> listQuestionId)
    {

        var questionGroupCounts = new Dictionary<QuestionGroup, int>
        {
            { QuestionGroup.R, 0 },
            { QuestionGroup.I, 0 },
            { QuestionGroup.A, 0 },
            { QuestionGroup.S, 0 },
            { QuestionGroup.E, 0 },
            { QuestionGroup.C, 0 }
        };


        var questions = await _context.Question
            .Where(a => listQuestionId.Contains(a.Id))
            .ToListAsync();


        foreach (var question in questions)
        {
            questionGroupCounts[question.Group]++;
        }


        int totalAnswers = questions.Count;

        if (totalAnswers == 0)
        {
            throw new InvalidOperationException("No questions found for the given IDs.");
        }

        var top3Groups = questionGroupCounts
            .Select(g => new { Group = g.Key, Percentage = ((double)g.Value / totalAnswers) * 100 })
            .OrderByDescending(g => g.Percentage)
            .Take(3)
            .ToList();

        string type = string.Join("", top3Groups.Select(g => g.Group.ToString()));

        var h_type = await _context.PersonalGroup
            .Where(p => p.Code.Equals(type))
            .FirstOrDefaultAsync();

        if (h_type == null)
        {
            throw new KeyNotFoundException("Personality type not found");
        }

         var majors = _context.MajorPersonalMatrix
        .Where(p => p.PersonalGroupId == h_type.Id)
        .Select(p => new MajorCategoryModel
        {
            Id = p.MajorCategory.Id,
            Name = p.MajorCategory.Name,
            Majors = p.MajorCategory.Majors.Select(m => new MajorModel
            {
                Id = m.Id,
                Name = m.Name,
                Code = m.Code
            }).ToList()
        })
        .ToList();

        var result = new PersonalGroupModel
        {
            Id = h_type.Id,
            Code = h_type.Code,
            Description = h_type.Description,
            Name = h_type.Name,
            Majors = majors,
            Percent = top3Groups.Select(g => new { g.Group, g.Percentage }).ToList()
        };

        return result;
    }
    #endregion

    #region Get By Id
    public async Task<IEnumerable<Question>> GetAllQuestionByTestId(Guid personalTestId)
    {
        var result = await _context.TestQuestion.Where(q => q.PersonalTestId == personalTestId).Select(p => p.Question).ToListAsync();
        return result;
    }

    public async Task<IEnumerable<Answer>> GetAnswerByQuestionId(int questionId)
    {
        var result = await _context.Answer.Where(a => a.QuestionId == questionId).ToListAsync();
        return result;
    }
    #endregion

    #region Check Type
    public async Task<TestTypeCode> CheckTestType(Guid personalTestId)
    {
        var testType = await _context.PersonalTest
            .Where(p => p.Id == personalTestId)
            .Select(p => p.TestType.TypeCode)
            .FirstOrDefaultAsync();

        return testType;
    }
    #endregion

    #region History Test
    public async Task<StudentHistoryModel> GetHistoryTestByStudentId(Guid studentId)
    {
        var tests = await _context.StudentTest
            .Where(s => s.StudentId == studentId)
            .Include(s => s.PersonalTest)               
            .ThenInclude(pt => pt.TestType)              
            .GroupBy(s => s.PersonalTestId)
            .Select(g => g.OrderByDescending(s => s.Date)
                          .Select(s => new StudentTest
                          {
                              Id = s.Id,
                              PersonalTestId = s.PersonalTestId,
                              PersonalTest = s.PersonalTest,
                              PersonalGroupId = s.PersonalGroupId,
                              PersonalGroup = s.PersonalGroup,
                              Date = s.Date
                          })
                          .FirstOrDefault())
            .ToListAsync();

        if (tests.Count == 0)
        {
            return new StudentHistoryModel
            {
                HistoryTests = new List<HistoryTestModel>(),
                HistoryMajor = new List<HistoryMajorModel>()
            };
        }

        var stTestId = tests.FirstOrDefault(t => t.PersonalTest?.TestType.TypeCode == TestTypeCode.Holland);


        var majorSelected = await _context.StudentChoice
            .Where(s => s.StudentTestId == stTestId.Id )
            .OrderByDescending(s => s.Rating)
            .Select(s => new HistoryMajorModel
            {
                MajorId = s.MajorOrOccupationId,
                MajorName = s.MajorOrOccupationName,
                Rating = s.Rating
            })
            .ToListAsync();
        var result = tests.Select(test => new HistoryTestModel
        {
            PersonalTestId = test.Id,
            PersonalTestName = test.PersonalTest?.Name ?? "N/A",
            PersonalGroupId = test.PersonalGroupId,
            PersonalGroupCode = test.PersonalGroup?.Code ?? "N/A",
            PersonalGroupName = test.PersonalGroup?.Name ?? "N/A",
            PersonalGroupDescription = test.PersonalGroup?.Description ?? "N/A",
            Date = test.Date
        }).ToList();

        return new StudentHistoryModel
        {
            HistoryTests = result,
            HistoryMajor = majorSelected
        };
    }

    #endregion

    #region Major by personal
    public async Task<IEnumerable<Major>> GetMajorsByPersonalGroupId(Guid personalGroupId)
    {
        var categoryIds = await _context.MajorPersonalMatrix
                                        .Where(x => x.PersonalGroupId == personalGroupId)
                                        .Select(x => x.MajorCategoryId)
                                        .ToListAsync();

        var majorsByCategories = await _context.Major
                                               .Where(x => categoryIds.Contains(x.MajorCategoryId))
                                               .GroupBy(x => x.MajorCategoryId)
                                               .ToListAsync();

        List<Major> majorChoices = new();
        Random random = new Random();
        foreach (var majorGroup in majorsByCategories)
        {
            var top5Majors = majorGroup.OrderBy(x => random.Next()).Take(5).ToList();
            majorChoices.AddRange(top5Majors);
        }

        return (majorChoices);
    }
    #endregion

    #region Occupation by major
    public async Task<IEnumerable<Occupation>> GetOccupationByMajorId(Guid majorId)
    {
        //var majorCategory = await _context.Major
        //    .Where(m => m.Id == majorId)
        //    .Select(m => m.MajorCategoryId)
        //    .FirstOrDefaultAsync();


        //if (majorCategory == Guid.Empty)
        //{
        //    return Enumerable.Empty<Occupation>();
        //}

        //var occupationGroupIds = await _context.MajorOccupationMatrix
        //    .Where(o => o.MajorCategoryId == majorCategory)
        //    .Select(o => o.OccupationalGroupId)
        //    .ToListAsync();

        //var occupations = await _context.Occupation
        //    .Where(o => occupationGroupIds.Contains(o.OccupationalGroupId))
        //    .ToListAsync();

        //var groupedOccupations = occupations.GroupBy(o => o.OccupationalGroupId);

        //List<Occupation> randomOccupations = new List<Occupation>();
        //Random random = new Random();


        //foreach (var group in groupedOccupations)
        //{

        //    var topOccupations = group.OrderBy(x => random.Next()).Take(5);
        //    randomOccupations.AddRange(topOccupations);
        //}

        //return randomOccupations;
        var occupations = await _context.Major
                .Where(m => m.Id == majorId)
                .SelectMany(m => m.MajorCategory.MajorOccupationMatrix)
                .SelectMany(mo => mo.OccupationalGroup.Occupations)
                .ToListAsync();
        return occupations;
    }
    #endregion

    #region Create Student Choice, Get top selected 
    public async Task<List<stChoiceModel>> CreateStudentChoice(StudentChoiceModel StModel, StudentChoiceType type)
    {

        try
        {
            var studentChoices = StModel.models.Select(m => new StudentChoice
            {
                StudentTestId = StModel.StudentTestId,
                MajorOrOccupationId = m.Id,
                MajorOrOccupationName = m.Name,
                Rating = m.Rating,
                Type = type
            }).ToList();

            await _context.StudentChoice.AddRangeAsync(studentChoices);
            await _context.SaveChangesAsync();


            var maxRating = studentChoices.Max(x => x.Rating);

            var topRatedChoices = studentChoices
                .Where(x => x.Rating == maxRating)
                .ToList();

            var majorIds = topRatedChoices.Select(x => x.MajorOrOccupationId).ToList();



            var groupedMajorsFromDb = await _context.Major
                 .Where(m => majorIds.Contains(m.Id))
                 .GroupBy(m => m.MajorCategoryId)
                 .Select(g => new
                 {
                     MajorCategoryId = g.Key,
                     MajorIds = g.Select(m => m.Id).ToList(),
                     MajorNames = g.Select(m => m.Name).ToList(),
                     MajorImage = g.Select(m => m.Image).ToList()
                 })
                 .ToListAsync();


            var groupedMajors = groupedMajorsFromDb.Select(g => new
            {
                MajorCategoryId = g.MajorCategoryId,
                MajorOrOccupationId = g.MajorIds.First(),
                MajorOrOccupationName = string.Join(", ", g.MajorNames.Distinct()),
                MajorOrOccupationImage = g.MajorImage.First()
            }).ToList();


            var distinctTopRatedChoices = topRatedChoices
                .Where(x => groupedMajors.Select(g => g.MajorOrOccupationId).Contains(x.MajorOrOccupationId))
                .GroupBy(x => x.MajorOrOccupationId)
                .Select(g => new stChoiceModel
                {
                    MajorOrOccupationId = g.Key,
                    MajorOrOccupationName = groupedMajors
                        .FirstOrDefault(m => m.MajorOrOccupationId == g.Key)?.MajorOrOccupationName ?? string.Empty,
                    StudentTestId = g.First().StudentTestId,
                    Type = g.First().Type,
                    Image = groupedMajors
                        .FirstOrDefault(m => m.MajorOrOccupationId == g.Key)?.MajorOrOccupationImage ?? string.Empty,
                })
                .ToList();

            return distinctTopRatedChoices;
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }


    #endregion


}


