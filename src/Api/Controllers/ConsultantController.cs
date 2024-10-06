using Api.Constants;
using Application.Interface.Service;
using Domain.Model.Consultant;
using Domain.Model.Student;
using Infrastructure.Persistence.Service;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/consultants")]
    [ApiController]
    public class ConsultantController : ControllerBase
    {
        private readonly IConsultantService _consultantService;
        public ConsultantController(IConsultantService consultantService)
        {
            _consultantService = consultantService;
        }

        [HttpGet(ApiEndPointConstant.Consultant.ConsultantEndpoint)]
        public async Task<IActionResult> GetConsultantByIdAsync(Guid id)
        {
            var result = await _consultantService.GetConsultantByIdAsync(id);
            return Ok(result);
        }

        [HttpGet(ApiEndPointConstant.Consultant.ConsultantsEndpoint)]
        public async Task<IActionResult> GetListConsultantsWithPaginateAsync([FromQuery] ConsultantSearchModel searchModel)
        {
            var result = await _consultantService.GetListConsultantsWithPaginateAsync(searchModel);
            return Ok(result);
        }

        [HttpPost(ApiEndPointConstant.Consultant.ConsultantsEndpoint)]
        public async Task<IActionResult> CreateConsultantAsyns([FromForm] ConsultantPostModel postModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var result = await _consultantService.CreateConsultantAsync(postModel);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut(ApiEndPointConstant.Consultant.ConsultantEndpoint)]
        public async Task<IActionResult> UpdateConsultantAsync([FromForm] ConsultantPutModel putModel, Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {

                var result = await _consultantService.UpdateConsultantAsync(id, putModel);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete(ApiEndPointConstant.Consultant.ConsultantEndpoint)]
        public async Task<IActionResult> DeleteConsultantAsync(Guid id)
        {
            var result = await _consultantService.DeleteConsultantAsync(id);
            return Ok(result);
        }

    }
}
