using Api.Constants;
using Api.Services;
using Api.Validators;
using Application.Interface.Service;
using Domain.Enum;
using Domain.Model.Test;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

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
        public async Task<IActionResult> CreateResultTest(StudentTestResultModel result)
        {

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

        //[CustomAuthorize(RoleEnum.Admin,RoleEnum.Student)]
        [HttpGet(ApiEndPointConstant.PersonalTest.PersonalTestEndpoint)]
        public async Task<IActionResult> GetPersonalTestById(Guid id)
        {
            try
            {

                //var cacheKey = RedisConstants.PersonalTestId + id;
                //var cacheResponse = await _cacheService.GetCacheResponseAsync<string>(cacheKey);
                var response = new PersonalTestModel();
                //if (cacheResponse != null)
                //{
                //    response = JsonConvert.DeserializeObject<PersonalTestModel>(cacheResponse);
                //    return Ok(response);
                //}
                //else
                //{
                    response = await _studentTestService.GetTestById(id);
                //    await _cacheService.SetCacheResponseAsync(cacheKey, response, TimeSpan.FromMinutes(16));
                    return Ok(response);
                //}

            }catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        //[CustomAuthorize(RoleEnum.Admin, RoleEnum.Student)]
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
        //[CustomAuthorize(RoleEnum.Admin, RoleEnum.Student)]
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

        [HttpGet(ApiEndPointConstant.PersonalTest.GetMajorsByPersonalGroupIdEndpoint)]
        public async Task<IActionResult> GetMajorAndOccupationByPersonalGroupId(Guid id)
        {
            try
            {
                var response = await _studentTestService.GetMajorsByPersonalGroupId(id);
                return Ok(response);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost(ApiEndPointConstant.PersonalTest.FilterMajorAndUniversityEndpoint)]
        public async Task<IActionResult> FilterMajorAndUniversity(FilterMajorAndUniversityModel model)
        {
            try
            {
                var response = await _studentTestService.FilterMajorAndUniversity(model);
                return Ok(response);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


    }
}
