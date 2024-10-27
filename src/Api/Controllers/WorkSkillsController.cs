using Api.Constants;
using Application.Interface.Service;
using Domain.Model.WorkSkills;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    public class WorkSkillsController : ControllerBase
    {
        private readonly IWorkSkillsService _workSkillsService;
        public WorkSkillsController(IWorkSkillsService workSkillsService)
        {
            _workSkillsService = workSkillsService;
        }


        //[CustomAuthorize(RoleEnum.Admin)]
        [HttpGet(ApiEndPointConstant.WorkSkill.WorkSkillsEndpoint)]
        public async Task<IActionResult> GetListWorkSkillsWithPaginateAsync(WorkSkillsSearchModel searchModel)
        {
            var result = await _workSkillsService.GetListWorkSkillsWithPaginateAsync(searchModel);
            return Ok(result);
        }

        //[CustomAuthorize(RoleEnum.Admin)]
        [HttpGet(ApiEndPointConstant.WorkSkill.WorkSkillEndpoint)]
        public async Task<IActionResult> GetWorkSkillByIdAsync(Guid id)
        {
            try
            {
                var result = await _workSkillsService.GetWorkSkillByIdAsync(id);
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
        [HttpPost(ApiEndPointConstant.WorkSkill.WorkSkillsEndpoint)]
        public async Task<IActionResult> CreateWorkSkillAsync(WorkSkillsPostModel postModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var result = await _workSkillsService.CreateWorkSkillAsync(postModel);
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
        [HttpPut(ApiEndPointConstant.WorkSkill.WorkSkillEndpoint)]
        public async Task<IActionResult> UpdateWorkSkillAsync(WorkSkillsPutModel putModel, Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var result = await _workSkillsService.UpdateWorkSkillAsync(putModel, id);
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
        [HttpDelete(ApiEndPointConstant.WorkSkill.WorkSkillEndpoint)]
        public async Task<IActionResult> DeleteWorkSkillAsync(Guid id)
        {
            try
            {
                var result = await _workSkillsService.DeleteWorkSkillAsync(id);
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
