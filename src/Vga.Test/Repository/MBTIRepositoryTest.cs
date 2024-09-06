using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Interface;
using Application.Interface.Repository;
using Domain.Entity;
using Domain.Enum;
using Domain.Model.MBTI;
using Infrastructure.Data;
using Infrastructure.Persistence.Repository;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Vga.Test.Repository
{
    public class MBTIRepositoryTest
    {
        private readonly VgaDbContext _dbContext;
        private readonly ResultMBTITestRepository _repo;
        

        public MBTIRepositoryTest()
        {
            var options = new DbContextOptionsBuilder<VgaDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _dbContext = new VgaDbContext(options);
            SeedDatabase();

            _repo =  new ResultMBTITestRepository(_dbContext);
        }

        private void SeedDatabase()
        {
            var answerList = new List<Answer>
            {
            // E vs I
            new Answer { Id = 1, PersonalityType = PersonalityTypeMBTI.E },
            new Answer { Id = 2, PersonalityType = PersonalityTypeMBTI.I },
            new Answer { Id = 3, PersonalityType = PersonalityTypeMBTI.E },
            new Answer { Id = 4, PersonalityType = PersonalityTypeMBTI.I },
            new Answer { Id = 5, PersonalityType = PersonalityTypeMBTI.E },
        
            // S vs N
            new Answer { Id = 6, PersonalityType = PersonalityTypeMBTI.S },
            new Answer { Id = 7, PersonalityType = PersonalityTypeMBTI.N },
            new Answer { Id = 8, PersonalityType = PersonalityTypeMBTI.S },
            new Answer { Id = 9, PersonalityType = PersonalityTypeMBTI.N },
            new Answer { Id = 10, PersonalityType = PersonalityTypeMBTI.S },

            // T vs F
            new Answer { Id = 11, PersonalityType = PersonalityTypeMBTI.T },
            new Answer { Id = 12, PersonalityType = PersonalityTypeMBTI.F },
            new Answer { Id = 13, PersonalityType = PersonalityTypeMBTI.T },
            new Answer { Id = 14, PersonalityType = PersonalityTypeMBTI.F },
            new Answer { Id = 15, PersonalityType = PersonalityTypeMBTI.T },
        
            // J vs P
            new Answer { Id = 16, PersonalityType = PersonalityTypeMBTI.J },
            new Answer { Id = 17, PersonalityType = PersonalityTypeMBTI.P },
            new Answer { Id = 18, PersonalityType = PersonalityTypeMBTI.J },
            new Answer { Id = 19, PersonalityType = PersonalityTypeMBTI.P },
            new Answer { Id = 20, PersonalityType = PersonalityTypeMBTI.P }
            };

            _dbContext.answer.RemoveRange(_dbContext.answer);
            _dbContext.SaveChanges();

            _dbContext.answer.AddRange(answerList);
            _dbContext.SaveChanges();
        }

        [Fact]
        public async Task CalculateResultMBTITest_ShouldReturnCorrectResult()
        {
            List<int> listId = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20 };
            // Arrange
            var studentSelectedModel = new StudentSelectedModel
            {
                StudentId = Guid.NewGuid(),
                TestId = 1,
                AnswerId = listId,
                CreateAt = DateTime.Now
            };

            // Act
            var result = await _repo.CalculateResultMBTITest(studentSelectedModel);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(ResultPersonalityTypeMBIT.ESTP, result.Personality_Type);

        }
        [Fact]
        public async Task CalculateResultMBTITest_ShouldReturnCorrectResult_WhenOnlyOnePersonalityTypeIsDominant()
        {
            // Arrange
            var studentSelectedModel = new StudentSelectedModel
            {
                StudentId = Guid.NewGuid(),
                TestId = 2,
                AnswerId = new List<int> { 1, 4, 7, 10, 13, 16, 20, 8, 9, 10 }, 
                CreateAt = DateTime.Now
            };

            // Act
            var result = await _repo.CalculateResultMBTITest(studentSelectedModel);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(ResultPersonalityTypeMBIT.ISTJ, result.Personality_Type); 
        }
    }
}
