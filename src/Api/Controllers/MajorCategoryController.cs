using Api.Constants;
using Api.Validators;
using Application.Interface.Service;
using Domain.Enum;
using Domain.Model.MajorCategory;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    public class MajorCategoryController : ControllerBase
    {
        private readonly IMajorCategoryService _majorCategoryService;
        public MajorCategoryController(IMajorCategoryService majorCategoryService)
        {
            _majorCategoryService = majorCategoryService;
        }


        [Authorize]
        [HttpGet(ApiEndPointConstant.MajorCategory.MajorCategoriesEndpoint)]
        public async Task<IActionResult> GetListMajorCategorysWithPaginateAsync(MajorCategorySearchModel searchModel)
        {
            var result = await _majorCategoryService.GetListMajorCategorysWithPaginateAsync(searchModel);
            return Ok(result);
        }

        [Authorize]
        [HttpGet(ApiEndPointConstant.MajorCategory.MajorCategoryEndpoint)]
        public async Task<IActionResult> GetMajorCategoryByIdAsync(Guid id)
        {
            try
            {
                var result = await _majorCategoryService.GetMajorCategoryByIdAsync(id);
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
        [HttpPost(ApiEndPointConstant.MajorCategory.MajorCategoriesEndpoint)]
        public async Task<IActionResult> CreateMajorCategoryAsync(MajorCategoryPostModel postModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var result = await _majorCategoryService.CreateMajorCategoryAsync(postModel);
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
        [HttpPut(ApiEndPointConstant.MajorCategory.MajorCategoryEndpoint)]
        public async Task<IActionResult> UpdateMajorCategoryAsync(MajorCategoryPutModel putModel, Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var result = await _majorCategoryService.UpdateMajorCategoryAsync(putModel, id);
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
        [HttpDelete(ApiEndPointConstant.MajorCategory.MajorCategoryEndpoint)]
        public async Task<IActionResult> DeleteMajorCategoryAsync(Guid id)
        {
            try
            {
                var result = await _majorCategoryService.DeleteMajorCategoryAsync(id);
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
