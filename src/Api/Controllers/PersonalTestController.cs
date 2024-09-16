using Application.Interface.Service;
using Domain.Model.Test;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/personal-test")]
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

        [HttpPost]
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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPersonalTestById(int id)
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
    }
}
