using GSManager.Core.Abstractions.Services.Electricity;
using GSManager.Core.Auth;
using GSManager.Core.Models.DTOs.Entities;
using GSManager.Core.Models.DTOs.Filters;
using GSManager.Core.Models.DTOs.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GSManager.API.Controllers.Electricity;

[Route("api/electricity-meters")]
[ApiController]
[Authorize]
public class ElectricityMeterController(IElectricityMeterService electricityMeterService) : ControllerBase
{
    private readonly IElectricityMeterService _electricityMeterService = electricityMeterService;

    [HttpGet]
    [Authorize(Policy = Permissions.ViewElectricityMeters)]
    public async Task<IActionResult> GetElectricityMetersAsync(
        [FromQuery] ElectricityMeterFilterDto filter,
        [FromQuery] PagedRequestDto pagedRequest,
        CancellationToken cancellationToken)
    {
        var result = await _electricityMeterService.GetElectricityMetersAsync(filter, pagedRequest, cancellationToken);
        return Ok(result);
    }

    [HttpGet]
    [Authorize(Policy = Permissions.ViewElectricityMeters)]
    public async Task<IActionResult> GetElectricityMeterByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var result = await _electricityMeterService.GetElectricityMeterByIdAsync(id, cancellationToken);
        return Ok(result);
    }

    [HttpPost]
    [Authorize(Policy = Permissions.AddElectricityMeters)]
    public async Task<IActionResult> AddElectricityMeterAsync([FromBody] ElectricityMeterDto electricityMeterDto, CancellationToken cancellationToken)
    {
        var result = await _electricityMeterService.AddElectricityMeterAsync(electricityMeterDto, cancellationToken);
        return CreatedAtAction(nameof(GetElectricityMeterByIdAsync), new { id = result.Id }, result);
    }

    //[HttpDelete("{id:guid}")]
    //[Authorize(Policy = Permissions.DeleteElectricityMeters)]
    //public async Task<IActionResult> DeleteElectricityMeterAsync(Guid id, CancellationToken cancellationToken)
    //{
    //    await _electricityMeterService.DeleteElectricityMeterAsync(id, cancellationToken);
    //    return NoContent();
    //}
}
