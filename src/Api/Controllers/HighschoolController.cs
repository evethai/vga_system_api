using Api.Constants;
using Application.Interface.Service;
using Domain.Model.Highschool;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;
[Route("high-schools")]
[ApiController]
public class HighschoolController : ControllerBase
{
    private readonly IHighschoolService _highschoolService;
    public HighschoolController(IHighschoolService highschoolService)
    {
        _highschoolService = highschoolService;
    }
    [HttpGet(ApiEndPointConstant.HighSchool.HighSchoolsEndpoint)]
    public async Task<IActionResult> GetListHighschoolAsync([FromQuery] HighschoolSearchModel searchModel)
    {
        var result = await _highschoolService.GetListHighSchoolAsync(searchModel);
        return Ok(result);
    }
    [HttpGet(ApiEndPointConstant.HighSchool.HighSchoolEndpoint)]
    public async Task<IActionResult> GetHighschoolById(Guid id)
    {
        var result = await _highschoolService.GetHighschoolByIdAsync(id);
        return Ok(result);
    }
    [HttpPost(ApiEndPointConstant.HighSchool.HighSchoolsEndpoint)]
    public async Task<IActionResult> CreateHighschoolAsync([FromForm] HighschoolPostModel postModel)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        try
        {
            var result = await _highschoolService.CreateHighschoolAsync(postModel);
            return Ok(result);
        } catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    [HttpPut(ApiEndPointConstant.HighSchool.HighSchoolEndpoint)]
    public async Task<IActionResult> UpdateHighschoolAsync([FromForm] HighschoolPutModel putModel)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        try
        {

            var result = await _highschoolService.UpdateHighschoolAsync(putModel);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    //[HttpDelete("{id}")]
    //public async Task<IActionResult> DeleteHighschoolAsync(int id)
    //{
    //    var result = await _highschoolService.DeleteHighschool(id);
    //    return Ok(result);
    //}
}
