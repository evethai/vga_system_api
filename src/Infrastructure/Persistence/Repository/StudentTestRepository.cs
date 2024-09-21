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

        var answers = await _context.answer
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


        var m_Type = await _context.personal_group
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



    public async Task<PersonalTestModel> GetTestById(Guid personalTestId)
    {

        var test = await _context.personal_test
            .Where(p => p.Id == personalTestId)
            .Include(p => p.TestType)
            .Select(p => new
            {
                p.Id,
                p.TestTypeId,
                p.Name,
                p.Description,
                Questions = p.TestQuestions.Select(tq => new
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
            QuestionModels = test.Questions.Select(q => new QuestionModel
            {
                Id = q.Id,
                Content = q.Content,
                Group = q.Group,
                TestTypeId = q.TestTypeId,
                _answerModels = q.Answers
            }).ToList()
        };

        return model;
    }



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


        var questions = await _context.question
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

        var h_type = await _context.personal_group
            .Where(p => p.Code.Equals(type))
            .FirstOrDefaultAsync();

        if (h_type == null)
        {
            throw new KeyNotFoundException("Personality type not found");
        }

        var majors = await _context.major_type.Where(m => m.PersonalGroupId == h_type.Id)
            .Select(m => new MajorModel
            {
                Id = m.Major.Id,
                Code = m.Major.Code,
                Name = m.Major.Name,
                Description = m.Major.Description
            })
            .ToListAsync();



        var result = new PersonalGroupModel
        {
            Id = h_type.Id,
            Code = h_type.Code,
            Description = h_type.Description,
            Name = h_type.Name,
            Majors = majors
        };

        return result;


    }

    public async Task<IEnumerable<Question>> GetAllQuestionByTestId(Guid personalTestId)
    {
        var result = await _context.test_question.Where(q => q.PersonalTestId == personalTestId).Select(p => p.Question).ToListAsync();
        return result;
    }

    public async Task<IEnumerable<Answer>> GetAnswerByQuestionId(int questionId)
    {
        var result = await _context.answer.Where(a => a.QuestionId == questionId).ToListAsync();
        return result;
    }

    public async Task<TestTypeCode> CheckTestType(Guid personalTestId)
    {
        var testType = await _context.personal_test
            .Where(p => p.Id == personalTestId)
            .Select(p => p.TestType.TypeCode)
            .FirstOrDefaultAsync();

        return testType;
    }

    public async Task<IEnumerable<StudentTest?>> GetHistoryTestByStudentId(Guid studentId)
    {
        var tests = await _context.student_test
            .Where(s => s.StudentId == studentId)
            .GroupBy(s => s.PersonalTest)
            .Select(g => g.OrderByDescending(s => s.Date)
                          .Select(s => new StudentTest
                          {
                              PersonalTestId = s.PersonalTestId,
                              PersonalTest = s.PersonalTest,
                              PersonalGroupId = s.PersonalGroupId,
                              PersonalGroup = s.PersonalGroup,
                              Date = s.Date
                          })
                          .FirstOrDefault())
            .ToListAsync();

        return tests;
    }

}


