using Application.Common.Constants;
using Application.Interface;
using Application.Interface.Service;
using AutoMapper;
using Azure.Messaging;
using Domain.Entity;
using Domain.Enum;
using Domain.Model;
using Domain.Model.PersonalGroup;
using Domain.Model.Question;
using Domain.Model.Response;
using Domain.Model.Test;
using Microsoft.AspNetCore.Mvc;

namespace Infrastructure.Persistence.Service
{
    public class StudentTestService : IStudentTestService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public StudentTestService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        #region result for test
        public async Task<ResponseModel> CreateResultForTest(StudentTestResultModel result)
        {
            TestTypeCode type = await _unitOfWork.StudentTestRepository.CheckTestType(result.PersonalTestId);
            PersonalGroupModel? personalGroupModel = null;
            if (type == TestTypeCode.MBTI)
            {
                personalGroupModel = await _unitOfWork.StudentTestRepository.CalculateResultMBTITest(result.listAnswerId);
            }
            else if (type == TestTypeCode.Holland)
            {
                personalGroupModel = await _unitOfWork.StudentTestRepository.CalculateResultHollandTest(result.listQuestionId);
            }
            else
            {
                throw new KeyNotFoundException("Test type not found or not in system");
            }
            TestResultDataModel data = new TestResultDataModel
            {
                AnswerIds = result.listAnswerId,
                QuestionIds = result.listQuestionId,
            };
            var jsonResult = Newtonsoft.Json.JsonConvert.SerializeObject(data);

            StudentTestModel model = new StudentTestModel
            {
                StudentId = result.StudentId,
                PersonalTestId = result.PersonalTestId,
                Date = result.Date,
                JsonResult = jsonResult,
                PersonalGroupId = personalGroupModel.Id
            };
            try
            {
                var resultTest = _mapper.Map<StudentTest>(model);
                await _unitOfWork.StudentTestRepository.AddAsync(resultTest);
                await _unitOfWork.SaveChangesAsync();
            }
            catch(Exception e)
            {
                throw new KeyNotFoundException("Create result test failed" + e);
            }

            return new ResponseModel
            {
                Message = "Success",
                IsSuccess = true,
                Data = new
                {
                    Code = personalGroupModel.Code,
                    Name = personalGroupModel.Name,
                    Des = personalGroupModel.Description,
                    Majors = personalGroupModel.Majors,
                    Percent = personalGroupModel.Percent
                }
            };

        }
        #endregion

        public async Task<PersonalTestModel> GetTestById(Guid id)
        {
            var result = await _unitOfWork.StudentTestRepository.GetTestById(id);
            return result;

        }

        public async Task<IEnumerable<PersonalTestModel>> GetAllTest()
        {
            var result = await _unitOfWork.PersonalTestRepository.GetAllAsync();
            var testModels = _mapper.Map<IEnumerable<PersonalTestModel>>(result);
            return testModels;
        }

        public async Task<IEnumerable<HistoryTestModel?>> GetHistoryTestByStudentId(Guid studentId)
        {
            var tests = await _unitOfWork.StudentTestRepository.GetHistoryTestByStudentId(studentId);
            return tests;
        }

        public async Task<ResponseModel> GetMajorsOrOccByPersonalGroupId(Guid personalityGroupId, Guid studentId)
        {
            var result = await _unitOfWork.StudentTestRepository.GetMajorsByPersonalGroupId(personalityGroupId);
            if (result == null || !result.Any())
            {
                return new ResponseModel
                {
                    IsSuccess = false,
                    Message = MessagesConstants.MajorOrOccupationChoice.NoMajorsFound, 
                    Data = null
                };
            }

            List<MajorOrOccupationModel> models = new List<MajorOrOccupationModel>();
            foreach (var major in result)
            {
                models.Add(new MajorOrOccupationModel
                {
                    Id = major.Id,
                    Name = major.Name,
                    Type = StudentChoiceType.Major
                });
            }
            

            var studentTestId = await _unitOfWork.StudentTestRepository.SingleOrDefaultAsync(
                predicate: x => x.StudentId == studentId,
                orderBy: x => x.OrderByDescending(x => x.Date),
                selector: x => x.Id
            );

            //ok test
            var occupations = await _unitOfWork.StudentTestRepository.GetOccupationByMajorId(models[0].Id);


            if (studentTestId == Guid.Empty)
            {
                //return new ResponseModel
                //{
                //    IsSuccess = false,
                //    Message = MessagesConstants.MajorOrOccupationChoice.NoStudentTestFound,
                //    Data = null
                //};
                studentTestId = Guid.NewGuid();
            }

            return new ResponseModel
            {
                IsSuccess = true,
                Message = MessagesConstants.MajorOrOccupationChoice.MajorChoice, 
                Data = new 
                {
                    studentTestId = studentTestId,
                    models = models
                }
            };
        }

        public async Task<ResponseModel> FilterMajorAndUniversity(FilterMajorAndUniversityModel model)
        {
            var listMajorId = await _unitOfWork.StudentTestRepository.CreateStudentChoice(model.studentChoiceModel, StudentChoiceType.Major);
            var pre = _unitOfWork.AdmissionInformationRepository.BuildFilterAndOrderBy(model.filterInfor, listMajorId);
            var listUniversity = await _unitOfWork.AdmissionInformationRepository.GetByConditionAsync(pre.filter, pre.orderBy);

            var occupations = await _unitOfWork.StudentTestRepository.GetOccupationByMajorId(listMajorId[0]);

            return new ResponseModel
            {
                IsSuccess = true,
                Message = MessagesConstants.MajorOrOccupationChoice.FilterUniversity,
                Data = new
                {
                    universities = listUniversity,
                    occupations = occupations
                }
            };
        }





    }
}
