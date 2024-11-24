using Application.Common.Constants;
using Application.Interface;
using Application.Interface.Service;
using AutoMapper;
using Azure.Messaging;
using Domain.Entity;
using Domain.Enum;
using Domain.Model;
using Domain.Model.Major;
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
            var stTestId = "";
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
                var stTest = await _unitOfWork.StudentTestRepository.AddAsync(resultTest);
                await _unitOfWork.SaveChangesAsync();
                stTestId = stTest.Id.ToString();
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
                    stTestId = stTestId,
                    Code = personalGroupModel.Code,
                    Name = personalGroupModel.Name,
                    Des = personalGroupModel.Description,
                    Majors = personalGroupModel.Majors,
                    Percent = personalGroupModel.Percent
                }
            };

        }
        #endregion

        #region get test
        public async Task<PersonalTestModel> GetTestById(Guid id)
        {
            var result = await _unitOfWork.StudentTestRepository.GetTestById(id);
            return result;

        }
        #endregion

        #region get all test
        public async Task<IEnumerable<PersonalTestModel>> GetAllTest()
        {
            var result = await _unitOfWork.PersonalTestRepository.GetAllAsync();
            var testModels = _mapper.Map<IEnumerable<PersonalTestModel>>(result);
            return testModels;
        }
        #endregion

        #region get history test by student id
        public async Task<StudentHistoryModel> GetHistoryTestByStudentId(Guid studentId)
        {
            var tests = await _unitOfWork.StudentTestRepository.GetHistoryTestByStudentId(studentId);
            return tests;
        }
        #endregion


        public async Task<ResponseModel> GetMajorsByPersonalGroupId(Guid stTestId)
        {
            var personalityGroupId = await _unitOfWork.StudentTestRepository.SingleOrDefaultAsync(predicate: x => x.Id == stTestId, selector: x=> x.PersonalGroupId);
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
                    //Type = StudentChoiceType.Major
                });
            }
            
            return new ResponseModel
            {
                IsSuccess = true,
                Message = MessagesConstants.MajorOrOccupationChoice.MajorChoice, 
                Data = models
            };
        }

        public async Task<ResponseModel> FilterMajorAndUniversity(FilterMajorAndUniversityModel model)
        {
            var stChoices = await _unitOfWork.StudentTestRepository.CreateStudentChoice(model.studentChoiceModel, StudentChoiceType.Major);
            if(stChoices == null || !stChoices.Any())
            {
                return new ResponseModel
                {
                    IsSuccess = false,
                    Message = MessagesConstants.MajorOrOccupationChoice.NoMajorsFound,
                    Data = null
                };
            }
            List<ResultAfterRatingModel> result = new();
            foreach (var choices in stChoices)
            {
                var occupations = await _unitOfWork.StudentTestRepository.GetOccupationByMajorId(choices.MajorOrOccupationId);
                var condition = _unitOfWork.AdmissionInformationRepository.BuildFilterAndOrderBy(model.filterInfor, choices);
                var universities = await _unitOfWork.AdmissionInformationRepository.GetByConditionAsync(condition.filter, condition.orderBy);
                result.Add(new ResultAfterRatingModel
                {
                    MajorId = choices.MajorOrOccupationId,
                    MajorName = choices.MajorOrOccupationName,
                    Image = choices.Image,
                    _occupations = _mapper.Map<List<OccupationByMajorIdModel>>(occupations),
                    _universities = _mapper.Map<List<UniversityByMajorIdModel>>(universities)
                });
            }
            

            return new ResponseModel
            {
                IsSuccess = true,
                Message = MessagesConstants.MajorOrOccupationChoice.FilterUniversity,
                Data = result
            };
        }


    }
}
