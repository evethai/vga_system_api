using Api.Constants;
using Api.Services;
using Api.Validators;
using Application.Interface.Service;
using Domain.Enum;
using Domain.Model.PersonalTest;
using Domain.Model.StudentChoice;
using Domain.Model.Test;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Api.Controllers
{
    [Route("/personal-test")]
    [ApiController]
    //[CustomAuthorize(RoleEnum.Admin, RoleEnum.Student)]
    public class PersonalTestController : ControllerBase
    {
        private readonly IStudentTestService _studentTestService;
        private readonly IWalletService _walletService;
        private readonly IPersonalTestService _personalTestService;
        private readonly ICacheService _cacheService;
        private readonly IStudentChoiceService _studentChoiceService;


        public PersonalTestController(IStudentTestService studentTestService, IWalletService walletService, IPersonalTestService personalTestService, ICacheService cacheService, IStudentChoiceService studentChoiceService)
        {
            _studentTestService = studentTestService;
            _walletService = walletService;
            _personalTestService = personalTestService;
            _cacheService = cacheService;
            _studentChoiceService = studentChoiceService;

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

        [HttpGet(ApiEndPointConstant.PersonalTest.PersonalTestEndpoint)]
        public async Task<IActionResult> GetPersonalTestById(Guid id, Guid accountId)
        {
            try
            {
                var cacheKey = RedisConstants.PersonalTestId + id;
                var cacheResponse = await _cacheService.GetCacheResponseAsync<string>(cacheKey);
                PersonalTestModel response;

                if (cacheResponse != null)
                {
                    response = JsonConvert.DeserializeObject<PersonalTestModel>(cacheResponse);
                }
                else
                {
                    response = await _studentTestService.GetTestById(id);

                    if (response == null)
                    {
                        return Ok("Test does not exist!");
                    }
                    await _cacheService.SetCacheResponseAsync(cacheKey, response, TimeSpan.FromMinutes(16));
                }
                var transaction = await _walletService.UpdateWalletUsingByTestAsync(accountId, response.Point);

                if (!transaction.IsSuccess)
                {
                    return Ok("Error transaction with point!");
                }

                return Ok(response);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet(ApiEndPointConstant.PersonalTest.PersonalTestsEndpoint)]
        public async Task<IActionResult> GetAllTest([FromForm] PersonalTestSearchModel model)
        {
            try
            {
                var response = await _studentTestService.GetAllTest(model);
                return Ok(response);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet(ApiEndPointConstant.PersonalTest.GetHistoryUserTestEndpoint)]
        public async Task<IActionResult> GetHistoryTestByStudentId(Guid id)
        {
            try
            {
                var response = await _studentTestService.GetHistoryTestByStudentId(id);
                if (response == null)
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
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var result = await _studentTestService.FilterOccupationAndUniversity(model);
                if (!result.IsSuccess)
                {
                    return BadRequest(result.Message);
                }
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost(ApiEndPointConstant.PersonalTest.PersonalTestsEndpoint)]
        public async Task<IActionResult> CreatePersonalTest([FromForm] PersonalTestPostModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var result = await _personalTestService.CreatePersonalTest(model);
                if (!result.IsSuccess)
                {
                    return BadRequest(result.Message);
                }
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut(ApiEndPointConstant.PersonalTest.PersonalTestEndpoint)]
        public async Task<IActionResult> UpdatePersonalTest(Guid id, PersonalTestPutModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var response = await _personalTestService.UpdatePersonalTest(id, model);
                if (!response.IsSuccess)
                {
                    return BadRequest(response.Message);
                }
                return Ok(response);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpDelete(ApiEndPointConstant.PersonalTest.PersonalTestEndpoint)]
        public async Task<IActionResult> DeletePersonalTest(Guid id)
        {
            try
            {
                var response = await _personalTestService.DeletePersonalTest(id);
                if (!response.IsSuccess)
                {
                    return BadRequest(response.Message);
                }
                return Ok(response);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

        }

        [HttpGet(ApiEndPointConstant.PersonalTest.GetStudentCareEndpoint)]
        public async Task<IActionResult> GetStudentCareById (Guid id)
        {
            var result = await _studentChoiceService.GetAllStudentCareById(id);
            return Ok(result);
        }
        [HttpPost(ApiEndPointConstant.PersonalTest.StudentCareEndpoint)]
        public async Task<IActionResult> CreateStudentCare (StudentChoicePostModel model)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var result = await _studentChoiceService.CreateNewStudentCare(model);
                if (!result.IsSuccess)
                {
                    return BadRequest(result.Message);
                }
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
