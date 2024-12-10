using Api.Constants;
using Api.Validators;
using Application.Interface.Service;
using Domain.Enum;
using Domain.Model.Occupation;
using Infrastructure.Persistence.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    public class OccupationController : ControllerBase
    {
        private readonly IOccupationService _occupationService;
        public OccupationController(IOccupationService occupationService)
        {
            _occupationService = occupationService;
        }

        [Authorize]
        [HttpGet(ApiEndPointConstant.Occupation.OccupationsEndpoint)]
        public async Task<IActionResult> GetListOccupationsWithPaginateAsync(OccupationSearchModel searchModel)
        {
            var result = await _occupationService.GetListOccupationsWithPaginateAsync(searchModel);
            return Ok(result);
        }

        [Authorize]
        [HttpGet(ApiEndPointConstant.Occupation.OccupationEndpoint)]
        public async Task<IActionResult> GetOccupationByIdAsync(Guid id, Guid studentId)
        {
            try
            {
                var result = await _occupationService.GetOccupationByIdAsync(id,studentId);
                return (result.IsSuccess == false)
                    ? BadRequest(result)
                    : Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [CustomAuthorize(RoleEnum.Admin)]
        [HttpPost(ApiEndPointConstant.Occupation.OccupationsEndpoint)]
        public async Task<IActionResult> CreateOccupationAsync(OccupationPostModel postModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var result = await _occupationService.CreateOccupationAsync(postModel);
                return (result.IsSuccess == false)
                    ? BadRequest(result)
                    : Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [CustomAuthorize(RoleEnum.Admin)]
        [HttpPut(ApiEndPointConstant.Occupation.OccupationEndpoint)]
        public async Task<IActionResult> UpdateOccupationAsync(OccupationPutModel putModel, Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var result = await _occupationService.UpdateOccupationAsync(id, putModel);
                return (result.IsSuccess == false)
                    ? BadRequest(result)
                    : Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [CustomAuthorize(RoleEnum.Admin)]
        [HttpDelete(ApiEndPointConstant.Occupation.OccupationEndpoint)]
        public async Task<IActionResult> DeleteOccupationAsync(Guid id)
        {
            try
            {
                var result = await _occupationService.DeleteOccupationAsync(id);
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
