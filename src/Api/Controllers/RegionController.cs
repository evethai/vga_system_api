using Application.Interface.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;
[Route("api/region")]
[ApiController]
public class RegionController : ControllerBase
{
    private readonly IRegionService _regionService;
    public RegionController(IRegionService regionService)
    {
        _regionService = regionService;
    }
    [HttpGet]
    public async Task<IActionResult> GetListRegionAsync()
    {
        var result = await _regionService.GetAllRegion();
        return Ok(result);
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> GetRegionById(int id)
    {
        var result = await _regionService.GetRegionById(id);
        return Ok(result);
    }
}
