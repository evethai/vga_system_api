using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Interface.Repository;
using Domain.Entity;
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
        public async Task<ResponseModel> UpdateQuestion(QuestionPutModel model)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var question = await _context.Question.FindAsync(model.Id);
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
                    question.Status = model.status;

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
        }
        #endregion

    }


}

