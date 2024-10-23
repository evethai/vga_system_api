using Api.Constants;
using Api.Validators;
using Application.Interface.Service;
using Domain.Enum;
using Domain.Model.Test;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("/personal-test")]
    [ApiController]
    public class PersonalTestController : ControllerBase
    {
        private readonly IStudentTestService _studentTestService;
        //private readonly ICacheService _cacheService;

        public PersonalTestController(IStudentTestService studentTestService)
        {
            _studentTestService = studentTestService;
            //_cacheService = cacheService;
        }

        [HttpPost(ApiEndPointConstant.PersonalTest.GetResultPersonalTestEndpoint)]
        public async Task<IActionResult> CreateResultMBTITest(StudentTestResultModel result)
        {
            //var cacheKey = RedisConstants.AnswerMBTIKey+ result.TestId;
            //var cacheResponse = await _cacheService.GetCacheResponseAsync<string>(cacheKey);
            //if (cacheResponse != null)
            //{
            //    listAnswer = JsonConvert.DeserializeObject<List<AnswerModel>>(cacheResponse);
            //}
            //var listAnswers = await _resultBMTITestService.GetListAnswerToRedis(result.TestId);
            //await _cacheService.SetCacheResponseAsync(cacheKey, listAnswers, TimeSpan.FromMinutes(16));

            try
            {
                var response = await _studentTestService.CreateResultForTest(result);
                return Ok(response);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

        }

        [CustomAuthorize(RoleEnum.Admin,RoleEnum.Student)]
        [HttpGet(ApiEndPointConstant.PersonalTest.PersonalTestEndpoint)]
        public async Task<IActionResult> GetPersonalTestById(Guid id)
        {
            //var cacheKey = RedisConstants.AnswerMBTIKey + id;
            //var listAnswer = await _resultBMTITestService.GetListAnswerToRedis(id);
            //await _cacheService.SetCacheResponseAsync(cacheKey, listAnswer, TimeSpan.FromMinutes(16));

            try
            {
                var response = await _studentTestService.GetTestById(id);
                return Ok(response);
            }catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        [CustomAuthorize(RoleEnum.Admin, RoleEnum.Student)]
        [HttpGet(ApiEndPointConstant.PersonalTest.PersonalTestsEndpoint)]
        public async Task<IActionResult> GetAllTest()
        {
            try
            {
                var response = await _studentTestService.GetAllTest();
                return Ok(response);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [CustomAuthorize(RoleEnum.Admin, RoleEnum.Student)]
        [HttpGet(ApiEndPointConstant.PersonalTest.GetHistoryUserTestEndpoint)]
        public async Task<IActionResult> GetHistoryTestByStudentId(Guid id)
        {
            try
            {
                var response = await _studentTestService.GetHistoryTestByStudentId(id);
                if(response == null)
                {
                    return Ok("User does not take the test!");
                }
                return Ok(response);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("student/{id}")]
        public async Task<IActionResult> GetMajorAndOccupationByPersonalGroupId(Guid id, Guid perId)
        {
            try
            {
                var response = await _studentTestService.GetMajorsOrOccByPersonalGroupId(perId, id);
                return Ok(response);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
