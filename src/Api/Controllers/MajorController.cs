using Api.Constants;
using Application.Interface.Service;
using Domain.Model.Major;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    public class MajorController  : ControllerBase
    {
        private readonly IMajorService _majorService;
        public MajorController(IMajorService majorService)
        {
            _majorService = majorService;
        }

        //[CustomAuthorize(RoleEnum.Admin, RoleEnum.Student)]
        [HttpGet(ApiEndPointConstant.Major.MajorsEndpoint)]
        public async Task<IActionResult> GetListMajorsWithPaginateAsync(MajorSearchModel searchModel)
        {
            var result = await _majorService.GetListMajorsWithPaginateAsync(searchModel);
            return Ok(result);
        }

        //[CustomAuthorize(RoleEnum.Admin, RoleEnum.Student)]
        [HttpGet(ApiEndPointConstant.Major.MajorEndpoint)]
        public async Task<IActionResult> GetMajorByIdAsync(Guid id)
        {
            try
            {
                var result = await _majorService.GetMajorByIdAsync(id);
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
        [HttpPost(ApiEndPointConstant.Major.MajorsEndpoint)]
        public async Task<IActionResult> CreateMajorAsync(MajorPostModel postModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var result = await _majorService.CreateMajorAsync(postModel);
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
        [HttpPut(ApiEndPointConstant.Major.MajorEndpoint)]
        public async Task<IActionResult> UpdateMajorAsync(MajorPutModel putModel, Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var result = await _majorService.UpdateMajorAsync(putModel, id);
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
        [HttpDelete(ApiEndPointConstant.Major.MajorEndpoint)]
        public async Task<IActionResult> DeleteMajorAsync(Guid id)
        {
            try
            {
                var result = await _majorService.DeleteMajorAsync(id);
                return (result.IsSuccess == false)
                    ? BadRequest(result)
                    : Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        //[CustomAuthorize(RoleEnum.Student)]
        [HttpGet(ApiEndPointConstant.Major.MajorAndRelationEndpoint)]
        public async Task<IActionResult> OccupationAndUniversityByMajorId(Guid id)
        {
            try
            {
                var result = await _majorService.OccupationAndUniversityByMajorId(id);
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
