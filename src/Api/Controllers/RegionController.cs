using Api.Constants;
using Application.Interface.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;
[Route("api/regions")]
[ApiController]
public class RegionController : ControllerBase
{
    private readonly IRegionService _regionService;
    public RegionController(IRegionService regionService)
    {
        _regionService = regionService;
    }
    [Authorize]
    [HttpGet(ApiEndPointConstant.Region.RegionsEndpoint)]
    public async Task<IActionResult> GetListRegionAsync()
    {
        var result = await _regionService.GetAllRegion();
        return Ok(result);
    }
    [Authorize]
    [HttpGet(ApiEndPointConstant.Region.RegionEndpoint)]
    public async Task<IActionResult> GetRegionById(Guid id)
    {
        var result = await _regionService.GetRegionById(id);
        return Ok(result);
    }
}
