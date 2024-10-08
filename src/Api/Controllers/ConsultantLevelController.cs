using Api.Constants;
using Application.Interface.Service;
using Domain.Model.ExpertLevel;
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

        [HttpPost(ApiEndPointConstant.ConsultantLevel.ConsultantLevelsEndpoint)]
        public async Task<IActionResult> CreateConsultantLevelAsync([FromForm] ConsultantLevelPostModel postModel)
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

        [HttpPut(ApiEndPointConstant.ConsultantLevel.ConsultantLevelEndpoint)]
        public async Task<IActionResult> UpdateConsultantLevelAsync([FromForm] ConsultantLevelPutModel putModel, int id)
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

    }
}
