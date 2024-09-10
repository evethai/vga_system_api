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
using Domain.Model.Answer;
using Domain.Model.MBTI;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repository;

public class ResultMBTITestRepository: GenericRepository<Result>, IResultMBTITestRepository
{
    private readonly VgaDbContext _context;
    public ResultMBTITestRepository(VgaDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<ResultModel> CalculateResultMBTITest(StudentSelectedModel result, List<Answer> redis_Value)
    {

        var personalityCounts = new Dictionary<PersonalityTypeMBTI, int>
        {
            { PersonalityTypeMBTI.E, 0 },
            { PersonalityTypeMBTI.I, 0 },
            { PersonalityTypeMBTI.S, 0 },
            { PersonalityTypeMBTI.N, 0 },
            { PersonalityTypeMBTI.T, 0 },
            { PersonalityTypeMBTI.F, 0 },
            { PersonalityTypeMBTI.J, 0 },
            { PersonalityTypeMBTI.P, 0 }
        };
        List<Answer> answers = new List<Answer>();
        if(redis_Value != null)
        {
            answers = redis_Value;
        }
        else
        {
            answers = await _context.answer.Where(a => result.AnswerId.Contains(a.Id)).ToListAsync();
        }

        foreach (var answer in answers)
        {
            personalityCounts[answer.PersonalityType]++;
        }

        var mbtiType = string.Empty;
        mbtiType += (personalityCounts[PersonalityTypeMBTI.I] >= personalityCounts[PersonalityTypeMBTI.E]) ? "I" : "E";
        mbtiType += (personalityCounts[PersonalityTypeMBTI.S] >= personalityCounts[PersonalityTypeMBTI.N]) ? "S" : "N";
        mbtiType += (personalityCounts[PersonalityTypeMBTI.T] >= personalityCounts[PersonalityTypeMBTI.F]) ? "T" : "F";
        mbtiType += (personalityCounts[PersonalityTypeMBTI.J] >= personalityCounts[PersonalityTypeMBTI.P]) ? "J" : "P";

        string resultPersonalityType = Enum.Parse<ResultPersonalityTypeMBIT>(mbtiType).ToString();
        var m_Type = _context.mbti_personality.Where(p => p.Code.Contains(resultPersonalityType)).FirstOrDefault();
        if(m_Type == null)
        {
            throw new Exception("Personality type not found");
        }
        var resultModel = new ResultModel
        {
            StudentId = result.StudentId,
            TestId = result.TestId,
            PersonalityId = m_Type.Id,
            CreateAt = DateTime.Now,
            description = m_Type.PersonalityDescription
        };

        return resultModel;
    }

    public async Task CreateStudentSelected(StudentSelectedModel result)
    {
        foreach (var answerId in result.AnswerId)
        {
            var studentSelected = new StudentSelected
            {
                StudentId = result.StudentId,
                AnswerId = answerId,
                CreateAt = DateTime.Now
            };

            await _context.student_selected.AddAsync(studentSelected);
        }
        await _context.SaveChangesAsync();
    }


    public async Task<IEnumerable<MBTITestModel>> GetMBTITestById(int id)
    {
        var questionsWithAnswers = await _context.question
            .Where(p => p.TestId == id)
            .Include(q => q.Answers) 
            .ToListAsync();

        var listQuestion = questionsWithAnswers.Select(question => new MBTITestModel
        {
            QuestionId = question.Id,
            QuestionContent = question.Content,
            Answer = question.Answers.Select(answer => new MBTIAnswerModel
            {
                AnswerId = answer.Id,
                AnswerContent = answer.Content,
                PersonalityType = answer.PersonalityType
            }).ToList()
        }).ToList();

        return listQuestion;
    }

    public async Task<IEnumerable<Answer>> GetListAnswerToRedis(int TestId)
    {
        var listAnswer = await _context.answer.Where(a => a.Question.TestId == TestId).ToListAsync();
        return listAnswer;
    }
}

