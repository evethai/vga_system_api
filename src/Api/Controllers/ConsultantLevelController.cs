using Api.Constants;
using Api.Validators;
using Application.Interface.Service;
using Domain.Enum;
using Domain.Model.ConsultantLevel;
using Domain.Model.Student;
using Domain.Model.TimeSlot;
using Infrastructure.Persistence.Service;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/consultant-levels")]
    [ApiController]
    public class ConsultantLevelController : ControllerBase
    {
        private readonly IConsultantLevelService _consultantLevelService;
        public ConsultantLevelController(IConsultantLevelService consultantLevelService)
        {
            _consultantLevelService = consultantLevelService;
        }

        //[CustomAuthorize(RoleEnum.Admin)]
        [HttpGet(ApiEndPointConstant.ConsultantLevel.ConsultantLevelEndpoint)]
        public async Task<IActionResult> GetConsultantLevelByIdAsync(int id)
        {
            try
            {
                var result = await _consultantLevelService.GetConsultantLevelByIdAsync(id);
                return (result.IsSuccess == false)
                    ? BadRequest(result)
                    : Ok(result);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //[CustomAuthorize(RoleEnum.Admin)]
        [HttpPost(ApiEndPointConstant.ConsultantLevel.ConsultantLevelsEndpoint)]
        public async Task<IActionResult> CreateConsultantLevelAsync(ConsultantLevelPostModel postModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var result = await _consultantLevelService.CreateConsultantLevelAsync(postModel);
                return (result.IsSuccess == false)
                    ? BadRequest(result)
                    : Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //[CustomAuthorize(RoleEnum.Admin)]
        [HttpPut(ApiEndPointConstant.ConsultantLevel.ConsultantLevelEndpoint)]
        public async Task<IActionResult> UpdateConsultantLevelAsync(ConsultantLevelPutModel putModel, int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var result = await _consultantLevelService.UpdateConsultantLevelAsync(putModel, id);
                return (result.IsSuccess == false)
                    ? BadRequest(result)
                    : Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //[CustomAuthorize(RoleEnum.Admin)]
        [HttpDelete(ApiEndPointConstant.ConsultantLevel.ConsultantLevelEndpoint)]
        public async Task<IActionResult> DeleteConsultantLevelAsync(int id)
        {
            try
            {
                var result = await _consultantLevelService.DeleteConsultantLevelAsync(id);
                return (result.IsSuccess == false)
                    ? BadRequest(result)
                    : Ok(result);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        //[CustomAuthorize(RoleEnum.Admin, RoleEnum.Student)]
        [HttpGet(ApiEndPointConstant.ConsultantLevel.ConsultantLevelsEndpoint)]
        public async Task<IActionResult> GetListConsultantLevelWithPaginateAsync(ConsultantLevelSearchModel searchModel)
        {
            var result = await _consultantLevelService.GetListConsultantLevelWithPaginateAsync(searchModel);
            return Ok(result);
        }

    }
}
