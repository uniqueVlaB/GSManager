using FluentValidation;
using GSManager.Core.Abstractions.Repository;
using GSManager.Core.Abstractions.Services;
using GSManager.Core.Exceptions.Member;
using GSManager.Core.Exceptions.Priviledge;
using GSManager.Core.Exceptions.Role;
using GSManager.Core.Filters.Member;
using GSManager.Core.Mappers;
using GSManager.Core.Models.DTOs;
using GSManager.Core.Models.Entities.Society;
using Microsoft.EntityFrameworkCore;

namespace GSManager.Core.Services;

public class MemberService(
    IUnitOfWork unitOfWork,
    IValidator<MemberDto> validator
    ) : IMemberService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IValidator<MemberDto> _validator = validator;

    public async Task<ICollection<MemberDto>> GetAllMembersAsync(CancellationToken cancellationToken)
    {
        var members = await _unitOfWork.Members.GetAllAsync(
            cancellationToken, includeProperties: [nameof(Member.Plots)]);

        var dtos = members.Select(MemberMapper.ToDto).ToList();
        return dtos;
    }

    public async Task<ICollection<MemberDto>> GetFilteredMembersAsync(
        MemberFilterDto filter,
        CancellationToken cancellationToken)
    {
        var query = _unitOfWork.Members.GetQueryable()
            .Include(m => m.Plots);

        var pipeline = MemberFilterPipeline.Create();
        var filteredQuery = pipeline.Execute(query, filter);

        var members = await filteredQuery.ToListAsync(cancellationToken);
        return members.Select(MemberMapper.ToDto).ToList();
    }

    public async Task<MemberDto> GetMemberByIdAsync(Guid memberId, CancellationToken cancellationToken)
    {
        var member = await _unitOfWork.Members.GetAsync(
            m => m.Id == memberId,
            cancellationToken,
            includeProperties: [nameof(Member.Plots)]
            ) ?? throw new MemberNotFoundException(memberId);

        return MemberMapper.ToDto(member);
    }

    public async Task<MemberDto> AddMemberAsync(MemberDto memberDto, CancellationToken cancellationToken)
    {
        await ValidateMemberAsync(memberDto, cancellationToken);

        var role = await ResolveRoleAsync(memberDto.RoleId, cancellationToken);
        var priviledge = await ResolvePriviledgeAsync(memberDto.PriviledgeId, cancellationToken);
        var plots = await ResolvePlotsAsync(memberDto.PlotIds, cancellationToken);

        var member = MemberMapper.ToEntity(memberDto, role, priviledge, plots);

        _unitOfWork.Members.Add(member);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        memberDto.Id = member.Id;
        return memberDto;
    }

    public async Task DeleteMemberAsync(Guid memberId, CancellationToken cancellationToken)
    {
        var member = await _unitOfWork.Members.GetAsync(m => m.Id == memberId, cancellationToken)
            ?? throw new MemberNotFoundException(memberId);
        _unitOfWork.Members.Remove(member);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    private async Task ValidateMemberAsync(MemberDto memberDto, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(memberDto, cancellationToken);

        if (!validationResult.IsValid)
        {
            throw new InvalidMemberRequestException(validationResult.ToString());
        }
    }

    private async Task<Role?> ResolveRoleAsync(Guid? roleId, CancellationToken cancellationToken)
    {
        if (roleId is null)
        {
            return null;
        }

        var id = roleId.Value;
        var role = await _unitOfWork.Roles.GetAsync(r => r.Id == id, cancellationToken);
        return role ?? throw new RoleNotFoundException(id);
    }

    private async Task<Priviledge?> ResolvePriviledgeAsync(Guid? priviledgeId, CancellationToken cancellationToken)
    {
        if (priviledgeId is null)
        {
            return null;
        }

        var id = priviledgeId.Value;
        var priviledge = await _unitOfWork.Priviledges.GetAsync(p => p.Id == id, cancellationToken);
        return priviledge ?? throw new PriviledgeNotFoundException(id);
    }

    private async Task<List<Plot>?> ResolvePlotsAsync(IList<Guid>? plotIds, CancellationToken cancellationToken)
    {
        if (plotIds is null)
        {
            return null;
        }

        var plots = new List<Plot>();
        foreach (var plotId in plotIds)
        {
            var plot = await _unitOfWork.Plots.GetAsync(p => p.Id == plotId, cancellationToken)
            ?? throw new InvalidMemberRequestException($"Plot with Id {plotId} not found.");
            plots.Add(plot);
        }

        return plots;
    }
}
