using Api.Constants;
using Application.Interface.Service;
using Domain.Model.EntryLevelEducation;
using Domain.Model.TimeSlot;
using Infrastructure.Persistence.Service;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{

    [ApiController]
    public class EntryLevelEducationController : ControllerBase
    {
        private readonly IEntryLevelEducationService _entryLevelEducationService;
        public EntryLevelEducationController(IEntryLevelEducationService entryLevelEducationService)
        {
            _entryLevelEducationService = entryLevelEducationService;
        }


        //[CustomAuthorize(RoleEnum.Admin)]
        [HttpGet(ApiEndPointConstant.EntryLevelEducation.EntryLevelEducationsEndpoint)]
        public async Task<IActionResult> GetListEntryLevelsWithPaginateAsync(EntryLevelEducationSearchModel searchModel)
        {
            var result = await _entryLevelEducationService.GetListEntryLevelsWithPaginateAsync(searchModel);
            return Ok(result);
        }

        //[CustomAuthorize(RoleEnum.Admin)]
        [HttpGet(ApiEndPointConstant.EntryLevelEducation.EntryLevelEducationEndpoint)]
        public async Task<IActionResult> GetEntryLevelByIdAsync(Guid id)
        {
            try
            {
                var result = await _entryLevelEducationService.GetEntryLevelByIdAsync(id);
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
        [HttpPost(ApiEndPointConstant.EntryLevelEducation.EntryLevelEducationsEndpoint)]
        public async Task<IActionResult> CreateEntryLevelAsync(EntryLevelEducationPostModel postModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var result = await _entryLevelEducationService.CreateEntryLevelAsync(postModel);
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
        [HttpPut(ApiEndPointConstant.EntryLevelEducation.EntryLevelEducationEndpoint)]
        public async Task<IActionResult> UpdateEntryLevelAsync(EntryLevelEducationPutModel putModel, Guid entryLevelId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var result = await _entryLevelEducationService.UpdateEntryLevelAsync(putModel, entryLevelId);
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
        [HttpDelete(ApiEndPointConstant.EntryLevelEducation.EntryLevelEducationEndpoint)]
        public async Task<IActionResult> DeleteTimeSlotAsync(Guid id)
        {
            try
            {
                var result = await _entryLevelEducationService.DeleteEntryLevelAsync(id);
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
