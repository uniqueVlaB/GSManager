using FluentValidation;
using GSManager.Core.Abstractions.Repository;
using GSManager.Core.Abstractions.Services;
using GSManager.Core.Exceptions.Member;
using GSManager.Core.Exceptions.Priviledge;
using GSManager.Core.Exceptions.Role;
using GSManager.Core.Mappers;
using GSManager.Core.Models.DTOs;
using GSManager.Core.Models.Entities.Society;

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
            cancellationToken, includeProperties: [nameof(Member.Plots)]).ConfigureAwait(false);

        var dtos = members.Select(MemberMapper.ToDto).ToList();
        return dtos;
    }

    public async Task<MemberDto> GetMemberByIdAsync(Guid memberId, CancellationToken cancellationToken)
    {
        var member = await _unitOfWork.Members.GetAsync(
            m => m.Id == memberId,
            cancellationToken,
            includeProperties: [nameof(Member.Plots)]
            ).ConfigureAwait(false) ?? throw new MemberNotFoundException(memberId);

        return MemberMapper.ToDto(member);
    }

    public async Task<MemberDto> AddMemberAsync(MemberDto memberDto, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(memberDto, cancellationToken).ConfigureAwait(false);

        if (!validationResult.IsValid)
        {
            throw new InvalidMemberRequestException(validationResult.ToString());
        }

        Role? role = null;
        if (memberDto.RoleId is not null)
        {
            role = await _unitOfWork.Roles.GetAsync(r => r.Id == memberDto.RoleId.Value, cancellationToken).ConfigureAwait(false)
            ?? throw new RoleNotFoundException(memberDto.RoleId.Value);
        }

        Priviledge? priviledge = null;
        if (memberDto.PriviledgeId is not null)
        {
            priviledge = await _unitOfWork.Priviledges.GetAsync(p => p.Id == memberDto.PriviledgeId.Value, cancellationToken).ConfigureAwait(false)
            ?? throw new PriviledgeNotFoundException(memberDto.PriviledgeId.Value);
        }

        List<Plot>? plots = null;
        if (memberDto.PlotIds is not null)
        {
            plots = [];
            foreach (var plotId in memberDto.PlotIds)
            {
                var plot = await _unitOfWork.Plots.GetAsync(p => p.Id == plotId, cancellationToken).ConfigureAwait(false)
                ?? throw new InvalidMemberRequestException($"Plot with Id {plotId} not found.");
                plots.Add(plot);
            }
        }

        var member = MemberMapper.ToEntity(memberDto, role, priviledge, plots);

        _unitOfWork.Members.Add(member);
        await _unitOfWork.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        memberDto.Id = member.Id;
        return memberDto;
    }
}
