using Application.Interface;
using Application.Interface.Service;
using AutoMapper;
using Domain.Entity;
using Domain.Enum;
using Domain.Model;
using Domain.Model.PersonalGroup;
using Domain.Model.Question;
using Domain.Model.Response;
using Domain.Model.Test;

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
    }
}
