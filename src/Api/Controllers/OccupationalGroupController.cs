using Api.Constants;
using Api.Validators;
using Application.Interface.Service;
using Domain.Enum;
using Domain.Model.OccupationalGroup;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    public class OccupationalGroupController : ControllerBase
    {
        private readonly IOccupationalGroupService _occupationalGroupService;
        public OccupationalGroupController(IOccupationalGroupService occupationalGroupService)
        {
            _occupationalGroupService = occupationalGroupService;
        }

        [Authorize]
        [HttpGet(ApiEndPointConstant.OccupationalGroup.OccupationalGroupsEndpoint)]
        public async Task<IActionResult> GetListOccupationalGroupsWithPaginateAsync(OccupationalGroupSearchModel searchModel)
        {
            var result = await _occupationalGroupService.GetListOccupationalGroupsWithPaginateAsync(searchModel);
            return Ok(result);
        }

        [Authorize]
        [HttpGet(ApiEndPointConstant.OccupationalGroup.OccupationalGroupEndpoint)]
        public async Task<IActionResult> GetOccupationalGroupByIdAsync(Guid id)
        {
            try
            {
                var result = await _occupationalGroupService.GetOccupationalGroupByIdAsync(id);
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
        [HttpPost(ApiEndPointConstant.OccupationalGroup.OccupationalGroupsEndpoint)]
        public async Task<IActionResult> CreateOccupationalGroupAsync(OccupationalGroupPostModel postModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var result = await _occupationalGroupService.CreateOccupationalGroupAsync(postModel);
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
        [HttpPut(ApiEndPointConstant.OccupationalGroup.OccupationalGroupEndpoint)]
        public async Task<IActionResult> UpdateOccupationalGroupAsync(OccupationalGroupPutModel putModel, Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var result = await _occupationalGroupService.UpdateOccupationalGroupAsync(putModel, id);
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
        [HttpDelete(ApiEndPointConstant.OccupationalGroup.OccupationalGroupEndpoint)]
        public async Task<IActionResult> DeleteOccupationalGroupAsync(Guid id)
        {
            try
            {
                var result = await _occupationalGroupService.DeleteOccupationalGroupAsync(id);
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
