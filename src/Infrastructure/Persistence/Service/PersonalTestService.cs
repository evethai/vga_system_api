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
                    Message = "Model không thể là rỗng"
                };
            }

            try
            {
                var personalTest = _mapper.Map<PersonalTest>(model);
                personalTest.CreateAt = DateTime.UtcNow.AddHours(7);
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
                        Message = "TestTypeId không hợp lệ"
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
                            Message = "Dữ liệu Json cho các câu hỏi MBTI đang trống"
                        };
                    }

                    var result = await _unitOfWork.QuestionRepository.AddMbtiQuestions(personalTest.Id, model.TestTypeId, questions);
                    if (!result.IsSuccess)
                    {
                        return new ResponseModel
                        {
                            IsSuccess = false,
                            Message = result.Message
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
                            Message = "Dữ liệu Json cho các câu hỏi của Holland đang trống"
                        };
                    }

                    var result = await _unitOfWork.QuestionRepository.AddHollandQuestions(personalTest.Id, model.TestTypeId, questions);
                    if (!result.IsSuccess)
                    {
                        return new ResponseModel
                        {
                            IsSuccess = false,
                            Message = "Không thêm được câu hỏi của Holland"
                        };
                    }
                }

                await _unitOfWork.SaveChangesAsync();

                return new ResponseModel
                {
                    IsSuccess = true,
                    Message = "Bài kiểm tra cá nhân đã được tạo thành công",
                    Data = personalTest
                };
            }
            catch (JsonSerializationException ex)
            {
                return new ResponseModel
                {
                    IsSuccess = false,
                    Message = "Dữ liệu JSON không hợp lệ",
                    Data = ex.Message
                };
            }
            catch (Exception ex)
            {
                return new ResponseModel
                {
                    IsSuccess = false,
                    Message = "Đã xảy ra lỗi khi tạo bài kiểm tra cá nhân",
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
                    Message = "Không tìm thấy bài kiểm tra cá nhân"
                };
            }
            personalTest.Name = model.Name;
            personalTest.Description = model.Description;
            await _unitOfWork.PersonalTestRepository.UpdateAsync(personalTest);
            await _unitOfWork.SaveChangesAsync();

            return new ResponseModel
            {
                IsSuccess = true,
                Message = "Cập nhật thành công bài kiểm tra cá nhân",
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
                    Message = "Không tìm thấy bài kiểm tra cá nhân"
                };
            }
            personalTest.Status = false;
            await _unitOfWork.PersonalTestRepository.UpdateAsync(personalTest);
            await _unitOfWork.SaveChangesAsync();

            return new ResponseModel
            {
                IsSuccess = true,
                Message = "Xóa thành công kiểm tra cá nhân",
                Data = personalTest
            };
        }
    }
}
