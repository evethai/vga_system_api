using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Interface;
using Application.Interface.Service;
using AutoMapper;
using Domain.Entity;
using Domain.Enum;
using Domain.Model.PersonalTest;
using Domain.Model.Response;
using Newtonsoft.Json;

namespace Infrastructure.Persistence.Service
{
    public class PersonalTestService : IPersonalTestService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public PersonalTestService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ResponseModel> CreatePersonalTest(PersonalTestPostModel model)
        {
            if (model == null)
            {
                return new ResponseModel
                {
                    IsSuccess = false,
                    Message = "Model cannot be null"
                };
            }

            try
            {
                var personalTest = _mapper.Map<PersonalTest>(model);
                personalTest.CreateAt = DateTime.UtcNow;
                personalTest.Status = true;

                await _unitOfWork.PersonalTestRepository.AddAsync(personalTest);

                var type = await _unitOfWork.TestTypeRepository
                    .SingleOrDefaultAsync(
                        predicate: x => x.Id.Equals(model.TestTypeId),
                        selector: x => x.TypeCode
                    );

                if (type == null)
                {
                    return new ResponseModel
                    {
                        IsSuccess = false,
                        Message = "Invalid TestTypeId"
                    };
                }

                if (type == TestTypeCode.MBTI)
                {
                    var questions = JsonConvert.DeserializeObject<List<DataQuestionMBTIModel>>(model.JsonData);
                    if (questions == null || !questions.Any())
                    {
                        return new ResponseModel
                        {
                            IsSuccess = false,
                            Message = "Json data for MBTI questions is empty"
                        };
                    }

                    var result = await _unitOfWork.QuestionRepository.AddMbtiQuestions(personalTest.Id, model.TestTypeId, questions);
                    if (!result.IsSuccess)
                    {
                        return new ResponseModel
                        {
                            IsSuccess = false,
                            Message = "Failed to add MBTI questions"
                        };
                    }
                }
                else if (type == TestTypeCode.Holland)
                {
                    var questions = JsonConvert.DeserializeObject<List<DataQuestionHollandModel>>(model.JsonData);
                    if (questions == null || !questions.Any())
                    {
                        return new ResponseModel
                        {
                            IsSuccess = false,
                            Message = "Json data for Holland questions is empty"
                        };
                    }

                    var result = await _unitOfWork.QuestionRepository.AddHollandQuestions(personalTest.Id, model.TestTypeId, questions);
                    if (!result.IsSuccess)
                    {
                        return new ResponseModel
                        {
                            IsSuccess = false,
                            Message = "Failed to add Holland questions"
                        };
                    }
                }

                await _unitOfWork.SaveChangesAsync();

                return new ResponseModel
                {
                    IsSuccess = true,
                    Message = "Personal test created successfully",
                    Data = personalTest
                };
            }
            catch (JsonSerializationException ex)
            {
                return new ResponseModel
                {
                    IsSuccess = false,
                    Message = "Invalid JSON data",
                    Data = ex.Message
                };
            }
            catch (Exception ex)
            {
                return new ResponseModel
                {
                    IsSuccess = false,
                    Message = "An error occurred while creating personal test",
                    Data = ex.Message
                };
            }
        }
        public async Task<ResponseModel> UpdatePersonalTest(Guid id, PersonalTestPutModel model)
        {
            var personalTest = await _unitOfWork.PersonalTestRepository.GetByIdGuidAsync(id);
            if (personalTest == null)
            {
                return new ResponseModel
                {
                    IsSuccess = false,
                    Message = "Personal test not found"
                };
            }
            personalTest.Name = model.Name;
            personalTest.Description = model.Description;
            await _unitOfWork.PersonalTestRepository.UpdateAsync(personalTest);
            await _unitOfWork.SaveChangesAsync();

            return new ResponseModel
            {
                IsSuccess = true,
                Message = "Update personal test success",
                Data = personalTest
            };
        }

        public async Task<ResponseModel> DeletePersonalTest(Guid id)
        {
            var personalTest = await _unitOfWork.PersonalTestRepository.GetByIdGuidAsync(id);
            if (personalTest == null)
            {
                return new ResponseModel
                {
                    IsSuccess = false,
                    Message = "Personal test not found"
                };
            }
            personalTest.Status = false;
            await _unitOfWork.PersonalTestRepository.UpdateAsync(personalTest);
            await _unitOfWork.SaveChangesAsync();

            return new ResponseModel
            {
                IsSuccess = true,
                Message = "Delete personal test success",
                Data = personalTest
            };
        }
    }
}
