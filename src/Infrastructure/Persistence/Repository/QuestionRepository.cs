using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Application.Common.Extensions;
using Application.Interface;
using Application.Interface.Repository;
using Domain.Entity;
using Domain.Enum;
using Domain.Model.Consultant;
using Domain.Model.PersonalTest;
using Domain.Model.Question;
using Domain.Model.Response;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repository
{
    public class QuestionRepository : GenericRepository<Question>, IQuestionRepository
    {
        private readonly VgaDbContext _context;
        public QuestionRepository(VgaDbContext context) : base(context)
        {
            _context = context;
        }

        #region create question
        public async Task<ResponseModel> CreateQuestion(QuestionPostModel model)
        {
            var strategy = _context.Database.CreateExecutionStrategy();
            return await strategy.ExecuteAsync(async () =>
            {
                using (var transaction = await _context.Database.BeginTransactionAsync())
                {
                    try
                    {
                        var typeId = await _context.PersonalTest
                            .Where(p => p.Id == model.personalTestId)
                            .Select(p => p.TestTypeId)
                            .FirstOrDefaultAsync();

                        if (typeId == null)
                        {
                            throw new Exception("TestTypeId not found for the given PersonalTestId.");
                        }

                        Question question = new Question
                        {
                            TestTypeId = typeId,
                            Content = model.Content,
                            Group = model.Group,
                            Status = true,
                            CreateAt = DateTime.Now,
                        };

                        await _context.Question.AddAsync(question);
                        await _context.SaveChangesAsync();

                        TestQuestion testQuestion = new TestQuestion
                        {
                            PersonalTestId = model.personalTestId,
                            QuestionId = question.Id,
                            Status = true
                        };

                        await _context.TestQuestion.AddAsync(testQuestion);

                        if (model.Answers != null && model.Answers.Any())
                        {
                            List<Answer> answers = model.Answers.Select(answer => new Answer
                            {
                                Content = answer.Content,
                                AnswerValue = answer.AnswerValue,
                                QuestionId = question.Id,
                                status = true
                            }).ToList();

                            await _context.Answer.AddRangeAsync(answers);
                        }

                        await _context.SaveChangesAsync();
                        await transaction.CommitAsync();

                        return new ResponseModel
                        {
                            IsSuccess = true,
                            Message = "Question created successfully.",
                            Data = question.Id
                        };
                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync();

                        return new ResponseModel
                        {
                            IsSuccess = false,
                            Message = ex.Message
                        };
                    }
                }
            });
        }
        #endregion

        #region update question
        public async Task<ResponseModel> UpdateQuestion(QuestionPutModel model, int id)
        {
            var strategy = _context.Database.CreateExecutionStrategy();

            return await strategy.ExecuteAsync(async () =>
            {
                using (var transaction = await _context.Database.BeginTransactionAsync())
                {
                    try
                    {
                        var question = await _context.Question.FindAsync(id);
                        if (question == null)
                        {
                            return new ResponseModel
                            {
                                IsSuccess = false,
                                Message = "Question not found."
                            };
                        }

                        question.Content = model.Content;
                        question.Group = model.Group;

                        if (model.Answers != null)
                        {
                            foreach (var answer in model.Answers)
                            {
                                var existingAnswer = await _context.Answer.FindAsync(answer.Id);
                                if (existingAnswer != null)
                                {
                                    existingAnswer.Content = answer.Content;
                                    existingAnswer.AnswerValue = answer.AnswerValue;
                                    existingAnswer.status = true;
                                }
                            }
                        }

                        await _context.SaveChangesAsync();
                        await transaction.CommitAsync();

                        return new ResponseModel
                        {
                            IsSuccess = true,
                            Message = "Question updated successfully."
                        };
                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync();

                        return new ResponseModel
                        {
                            IsSuccess = false,
                            Message = ex.Message
                        };
                    }
                }
            });
        }

        #endregion

        //public async Task<ResponseModel> AddMbtiQuestions(Guid personalTestId, Guid testTypeId, List<DataQuestionMBTIModel> questions)
        //{
        //    if (questions == null || !questions.Any())
        //    {
        //        return new ResponseModel
        //        {
        //            IsSuccess = false,
        //            Message = "Questions cannot be null or empty."
        //        };
        //    }

        //    try
        //    {
        //        var questionEntities = questions.Select(item => new Question
        //        {
        //            Content = item.Content,
        //            Group = QuestionGroup.None,
        //            TestTypeId = testTypeId,
        //            CreateAt = DateTime.UtcNow.AddHours(7),
        //            Status = true
        //        }).ToList();
        //        await _context.Question.AddRangeAsync(questionEntities);
        //        await _context.SaveChangesAsync();


        //        var questionIdDictionary = questionEntities.ToDictionary(q => q.Content, q => q.Id);


        //        var answerEntities = questions.SelectMany(item => new List<Answer>
        //        {
        //            new Answer
        //            {
        //                Content = item.Answer1,
        //                QuestionId = questionIdDictionary[item.Content],  
        //                AnswerValue = item.Value1,
        //                status = true
        //            },
        //            new Answer
        //            {
        //                Content = item.Answer2,
        //                QuestionId = questionIdDictionary[item.Content],  
        //                AnswerValue = item.Value2,
        //                status = true
        //            }
        //        }).ToList();
        //        var testQuestionEntities = questionEntities.Select(question => new TestQuestion
        //        {
        //            PersonalTestId = personalTestId,
        //            QuestionId = question.Id,
        //            Status = true
        //        }).ToList();

        //        await _context.Answer.AddRangeAsync(answerEntities);
        //        await _context.TestQuestion.AddRangeAsync(testQuestionEntities);
        //        await _context.SaveChangesAsync();

        //        return new ResponseModel
        //        {
        //            IsSuccess = true,
        //            Message = "Added MBTI questions successfully."
        //        };
        //    }
        //    catch (Exception ex)
        //    {
        //        return new ResponseModel
        //        {
        //            IsSuccess = false,
        //            Message = $"An error occurred: {ex.Message}"
        //        };
        //    }
        //}
        public async Task<ResponseModel> AddMbtiQuestions(Guid personalTestId, Guid testTypeId, List<DataQuestionMBTIModel> questions)
        {
            if (questions == null || !questions.Any())
            {
                return new ResponseModel
                {
                    IsSuccess = false,
                    Message = "Questions cannot be null or empty."
                };
            }

            var duplicateQuestions = questions
                .GroupBy(q => q.Content)
                .Where(g => g.Count() > 1)
                .Select(g => g.Key)
                .ToList();

            if (duplicateQuestions.Any())
            {
                return new ResponseModel
                {
                    IsSuccess = false,
                    Message = $"Duplicate questions found: {string.Join(", ", duplicateQuestions)}"
                };
            }

            try
            {

                var questionEntities = questions.Select(item => new Question
                {
                    Content = item.Content,
                    Group = QuestionGroup.None,
                    TestTypeId = testTypeId,
                    CreateAt = DateTime.UtcNow.AddHours(7),
                    Status = true
                }).ToList();

                await _context.Question.AddRangeAsync(questionEntities);
                await _context.SaveChangesAsync();

                var questionIdDictionary = questionEntities.ToDictionary(q => q.Content, q => q.Id);

                var answerEntities = questions.SelectMany(item => new List<Answer>
                {
                    new Answer
                    {
                        Content = item.Answer1,
                        QuestionId = questionIdDictionary[item.Content],
                        AnswerValue = item.Value1,
                        status = true
                    },
                    new Answer
                    {
                        Content = item.Answer2,
                        QuestionId = questionIdDictionary[item.Content],
                        AnswerValue = item.Value2,
                        status = true
                    }
                }).ToList();


                var testQuestionEntities = questionEntities.Select(question => new TestQuestion
                {
                    PersonalTestId = personalTestId,
                    QuestionId = question.Id,
                    Status = true
                }).ToList();

                await _context.Answer.AddRangeAsync(answerEntities);
                await _context.TestQuestion.AddRangeAsync(testQuestionEntities);
                await _context.SaveChangesAsync();

                return new ResponseModel
                {
                    IsSuccess = true,
                    Message = "Added MBTI questions successfully."
                };
            }
            catch (Exception ex)
            {
                return new ResponseModel
                {
                    IsSuccess = false,
                    Message = $"An error occurred: {ex.Message}"
                };
            }
        }



        public async Task<ResponseModel> AddHollandQuestions(Guid personalTestId, Guid testTypeId, List<DataQuestionHollandModel> questions)
        {
            if (questions == null || !questions.Any())
            {
                return new ResponseModel
                {
                    IsSuccess = false,
                    Message = "Questions cannot be null or empty."
                };
            }

            try
            {

                var questionEntities = questions.Select(item => new Question
                {
                    Content = item.Content,
                    Group = item.Group,
                    TestTypeId = testTypeId,
                    CreateAt = DateTime.UtcNow.AddHours(7),
                    Status = true
                }).ToList();

                await _context.Question.AddRangeAsync(questionEntities);
                await _context.SaveChangesAsync(); 

 
                var testQuestionEntities = questionEntities.Select(question => new TestQuestion
                {
                    PersonalTestId = personalTestId,
                    QuestionId = question.Id, 
                    Status = true
                }).ToList();

                await _context.TestQuestion.AddRangeAsync(testQuestionEntities);
                await _context.SaveChangesAsync();


                return new ResponseModel
                {
                    IsSuccess = true,
                    Message = "Added Holland questions successfully."
                };
            }
            catch (Exception ex)
            {
                return new ResponseModel
                {
                    IsSuccess = false,
                    Message = $"An error occurred: {ex.Message}"
                };
            }
        }

    }


}

