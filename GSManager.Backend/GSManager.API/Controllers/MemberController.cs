using Microsoft.AspNetCore.Mvc;
using GSManager.Core.Abstractions.Services;
using GSManager.Core.Models.DTOs.Entities;
using GSManager.Core.Models.DTOs.Filters;
using GSManager.Core.Models.DTOs.Requests;

namespace GSManager.API.Controllers;

[ApiController]
[Route("members")]
public class MemberController(IMemberService memberService) : ControllerBase
{
    private readonly IMemberService _memberService = memberService;

    [HttpGet]
    public async Task<IActionResult> GetMembersAsync(
        [FromQuery] MemberFilterDto filterDto,
        [FromQuery] PagedRequestDto pagedRequest,
        CancellationToken cancellationToken)
    {
        var result = await _memberService.GetMembersAsync(filterDto, pagedRequest, cancellationToken);

        return Ok(result);
    }

    [HttpGet("select-list")]
    public async Task<IActionResult> GetMemberSelectListAsync(CancellationToken cancellationToken)
    {
        var selectList = await _memberService.GetMemberSelectListAsync(cancellationToken);
        return Ok(selectList);
    }

    [HttpGet("{memberId:guid}")]
    public async Task<IActionResult> GetMemberByIdAsync(Guid memberId, CancellationToken cancellationToken)
    {
        var member = await _memberService.GetMemberByIdAsync(memberId, cancellationToken);
        return Ok(member);
    }

    [HttpPost]
    public async Task<IActionResult> AddMemberAsync([FromBody] MemberDto memberDto, CancellationToken cancellationToken)
    {
        var createdMember = await _memberService.AddMemberAsync(memberDto, cancellationToken);
        return CreatedAtAction(nameof(AddMemberAsync), new { memberId = createdMember.Id }, createdMember);
    }

    [HttpDelete("{memberId:guid}")]
    public async Task<IActionResult> DeleteMemberAsync(Guid memberId, CancellationToken cancellationToken)
    {
        await _memberService.DeleteMemberAsync(memberId, cancellationToken);
        return NoContent();
    }

    [HttpPut("{memberId:guid}")]
    public async Task<IActionResult> UpdateMemberAsync(
        Guid memberId,
        [FromBody] MemberDto memberDto,
        CancellationToken cancellationToken)
    {
        var updatedMember = await _memberService.UpdateMemberAsync(memberId, memberDto, cancellationToken);
        return Ok(updatedMember);
    }
}
