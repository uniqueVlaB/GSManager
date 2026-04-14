using Microsoft.AspNetCore.Mvc;
using GSManager.Core.Models.DTOs.Entities;
using GSManager.Core.Models.DTOs.Filters;
using GSManager.Core.Models.DTOs.Requests;
using GSManager.API.Telemetry;
using Microsoft.AspNetCore.Authorization;
using GSManager.Core.Auth;
using GSManager.Core.Abstractions.Services.Society;

namespace GSManager.API.Controllers.Society;

[ApiController]
[Route("api/members")]
[Authorize]
public class MemberController(IMemberService memberService, ApiMeters metrics) : ControllerBase
{
    private readonly IMemberService _memberService = memberService;
    private readonly ApiMeters _metrics = metrics;

    [HttpGet]
    [Authorize(Policy = Permissions.ViewMembers)]
    public async Task<IActionResult> GetMembersAsync(
        [FromQuery] MemberFilterDto filterDto,
        [FromQuery] PagedRequestDto pagedRequest,
        CancellationToken cancellationToken)
    {
        var result = await _memberService.GetMembersAsync(filterDto, pagedRequest, cancellationToken);

        return Ok(result);
    }

    [HttpGet("select-list")]
    [Authorize(Policy = Permissions.ViewMembers)]
    public async Task<IActionResult> GetMemberSelectListAsync(CancellationToken cancellationToken)
    {
        var selectList = await _memberService.GetMemberSelectListAsync(cancellationToken);
        return Ok(selectList);
    }

    [HttpGet("{memberId:guid}")]
    [Authorize(Policy = Permissions.ViewMembers)]
    public async Task<IActionResult> GetMemberByIdAsync(Guid memberId, CancellationToken cancellationToken)
    {
        var member = await _memberService.GetMemberByIdAsync(memberId, cancellationToken);
        return Ok(member);
    }

    [HttpPost]
    [Authorize(Policy = Permissions.AddMembers)]
    public async Task<IActionResult> AddMemberAsync([FromBody] MemberDto memberDto, CancellationToken cancellationToken)
    {
        var createdMember = await _memberService.AddMemberAsync(memberDto, cancellationToken);
        _metrics.Member.Created(createdMember.Id.ToString());
        return CreatedAtAction(nameof(AddMemberAsync), new { memberId = createdMember.Id }, createdMember);
    }

    [HttpDelete("{memberId:guid}")]
    [Authorize(Policy = Permissions.DeleteMembers)]
    public async Task<IActionResult> DeleteMemberAsync(Guid memberId, CancellationToken cancellationToken)
    {
        await _memberService.DeleteMemberAsync(memberId, cancellationToken);
        _metrics.Member.Deleted();
        return NoContent();
    }

    [HttpPut("{memberId:guid}")]
    [Authorize(Policy = Permissions.EditMembers)]
    public async Task<IActionResult> UpdateMemberAsync(
        Guid memberId,
        [FromBody] MemberDto memberDto,
        CancellationToken cancellationToken)
    {
        var updatedMember = await _memberService.UpdateMemberAsync(memberId, memberDto, cancellationToken);
        return Ok(updatedMember);
    }
}
