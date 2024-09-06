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

    public async Task<ResultModel> CalculateResultMBTITest(StudentSelectedModel result)
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

        var answers = await _context.answer.Where(a => result.AnswerId.Contains(a.Id)).ToListAsync();

        foreach (var answer in answers)
        {
            personalityCounts[answer.PersonalityType]++;
        }

        var mbtiType = string.Empty;
        mbtiType += (personalityCounts[PersonalityTypeMBTI.I] >= personalityCounts[PersonalityTypeMBTI.E]) ? "I" : "E";
        mbtiType += (personalityCounts[PersonalityTypeMBTI.S] >= personalityCounts[PersonalityTypeMBTI.N]) ? "S" : "N";
        mbtiType += (personalityCounts[PersonalityTypeMBTI.T] >= personalityCounts[PersonalityTypeMBTI.F]) ? "T" : "F";
        mbtiType += (personalityCounts[PersonalityTypeMBTI.J] >= personalityCounts[PersonalityTypeMBTI.P]) ? "J" : "P";

        var resultPersonalityType = Enum.Parse<ResultPersonalityTypeMBIT>(mbtiType);

        var resultModel = new ResultModel
        {
            StudentId = result.StudentId,
            TestId = result.TestId,
            Personality_Type = resultPersonalityType,
            Personality_Description = "",
            CreateAt = DateTime.Now
        };

        return resultModel;
    }
}

