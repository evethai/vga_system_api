using System.Runtime.ConstrainedExecution;
using Api.Attributes;
using Api.Constants;
using Api.Services;
using Application.Interface.Service;
using Domain.Entity;
using Domain.Model.Answer;
using Domain.Model.MBTI;
using Infrastructure.Persistence.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Api.Controllers
{
    [Route("api/mbti-tests")]
    [ApiController]
    public class MBTIController : ControllerBase
    {
        private readonly IResultBMTITestService _resultBMTITestService;
        //private readonly ICacheService _cacheService;

        public MBTIController(IResultBMTITestService resultBMTITestService)
        {
            _resultBMTITestService = resultBMTITestService;
            //_cacheService = cacheService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateResultMBTITest(StudentSelectedModel result)
        {
            //var cacheKey = RedisConstants.AnswerMBTIKey+ result.TestId;
            //var cacheResponse = await _cacheService.GetCacheResponseAsync<string>(cacheKey);
            List<AnswerModel> listAnswer = null;
            //if (cacheResponse != null)
            //{
            //    listAnswer = JsonConvert.DeserializeObject<List<AnswerModel>>(cacheResponse);
            //}
            //var listAnswers = await _resultBMTITestService.GetListAnswerToRedis(result.TestId);
            //await _cacheService.SetCacheResponseAsync(cacheKey, listAnswers, TimeSpan.FromMinutes(16));

            var response = await _resultBMTITestService.CreateResultMBTITest(result,listAnswer);
            return Ok(response);

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetMBTITestById(int id)
        {
            //var cacheKey = RedisConstants.AnswerMBTIKey + id;
            //var listAnswer = await _resultBMTITestService.GetListAnswerToRedis(id);
            //await _cacheService.SetCacheResponseAsync(cacheKey, listAnswer, TimeSpan.FromMinutes(16));

            //return Ok(listAnswer);

            try
            {
                var result = await _resultBMTITestService.GetMBTITestById(id);
                if (result == null || !result.Any())
                {
                    return NotFound(new { Message = "MBTI Test not found for the given ID" });
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                // Log the error here
                return StatusCode(500, new { Message = "Internal server error", Details = ex.Message });
            }
        }
    }
}
