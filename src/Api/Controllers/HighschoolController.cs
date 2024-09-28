using Application.Interface.Service;
using Domain.Model.Highschool;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;
[Route("api/highschools")]
[ApiController]
public class HighschoolController : ControllerBase
{
    private readonly IHighschoolService _highschoolService;
    public HighschoolController(IHighschoolService highschoolService)
    {
        _highschoolService = highschoolService;
    }
    [HttpGet]
    public async Task<IActionResult> GetListHighschoolAsync([FromQuery] HighschoolSearchModel searchModel)
    {
        var result = await _highschoolService.GetListHighSchoolAsync(searchModel);
        return Ok(result);
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> GetHighschoolById(Guid id)
    {
        var result = await _highschoolService.GetHighschoolByIdAsync(id);
        return Ok(result);
    }
    [HttpPost]
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
    [HttpPut]
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
}
