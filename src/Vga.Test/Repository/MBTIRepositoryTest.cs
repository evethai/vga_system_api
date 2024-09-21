using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Interface;
using Application.Interface.Repository;
using Domain.Entity;
using Domain.Enum;
using Infrastructure.Data;
using Infrastructure.Persistence.Repository;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Vga.Test.Repository
{
    public class MBTIRepositoryTest
    {
        private readonly VgaDbContext _dbContext;
        private readonly StudentTestRepository _repo;
        

        public MBTIRepositoryTest()
        {
            var options = new DbContextOptionsBuilder<VgaDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            _dbContext = new VgaDbContext(options);
            SeedDatabase();

            _repo =  new StudentTestRepository(_dbContext);
        }

        private void SeedDatabase()
        {
            var answerList = new List<Answer>
            {
            // E vs I
            //new Answer { Id = 1, PersonalityType = PersonalityType.E },
            //new Answer { Id = 2, PersonalityType = PersonalityType.I },
            //new Answer { Id = 3, PersonalityType = PersonalityType.E },
            //new Answer { Id = 4, PersonalityType = PersonalityType.I },
            //new Answer { Id = 5, PersonalityType = PersonalityType.E },
        
            //// S vs N
            //new Answer { Id = 6, PersonalityType = PersonalityType.S },
            //new Answer { Id = 7, PersonalityType = PersonalityType.N },
            //new Answer { Id = 8, PersonalityType = PersonalityType.S },
            //new Answer { Id = 9, PersonalityType = PersonalityType.N },
            //new Answer { Id = 10, PersonalityType = PersonalityType.S },

            //// T vs F
            //new Answer { Id = 11, PersonalityType = PersonalityType.T },
            //new Answer { Id = 12, PersonalityType = PersonalityType.F },
            //new Answer { Id = 13, PersonalityType = PersonalityType.T },
            //new Answer { Id = 14, PersonalityType = PersonalityType.F },
            //new Answer { Id = 15, PersonalityType = PersonalityType.T },
        
            //// J vs P
            //new Answer { Id = 16, PersonalityType = PersonalityType.J },
            //new Answer { Id = 17, PersonalityType = PersonalityType.P },
            //new Answer { Id = 18, PersonalityType = PersonalityType.J },
            //new Answer { Id = 19, PersonalityType = PersonalityType.P },
            //new Answer { Id = 20, PersonalityType = PersonalityType.P }
            };

            _dbContext.answer.RemoveRange(_dbContext.answer);
            _dbContext.SaveChanges();

            _dbContext.answer.AddRange(answerList);
            _dbContext.SaveChanges();
        }

        [Fact]
        public async Task CalculateResultMBTITest_ShouldReturnCorrectResult()
        {
            //List<int> listId = new List<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20 };
            //// Arrange
            //var studentSelectedModel = new StudentSelectedModel
            //{
            //    StudentId = Guid.NewGuid(),
            //    TestId = 1,
            //    AnswerId = listId,
            //    CreateAt = DateTime.Now
            //};

            // Act
            //var result = await _repo.CalculateResultMBTITest(studentSelectedModel);

            // Assert
            //Assert.NotNull(result);
            //Assert.Equal(ResultPersonalityTypeMBIT.ESTP, result.PersonalityId);

        }
        [Fact]
        public async Task CalculateResultMBTITest_ShouldReturnCorrectResult_WhenOnlyOnePersonalityTypeIsDominant()
        {
            //// Arrange
            //var studentSelectedModel = new StudentSelectedModel
            //{
            //    StudentId = Guid.NewGuid(),
            //    TestId = 2,
            //    AnswerId = new List<int> { 1, 4, 7, 10, 13, 16, 20, 8, 9, 10 }, 
            //    CreateAt = DateTime.Now
            //};

            // Act
            //var result = await _repo.CalculateResultMBTITest(studentSelectedModel);

            // Assert
            //Assert.NotNull(result);
            //Assert.Equal(ResultPersonalityTypeMBIT.ISTJ, result.PersonalityId); 
        }
    }
}
