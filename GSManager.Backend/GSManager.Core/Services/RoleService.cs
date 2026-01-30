using FluentValidation;
using GSManager.Core.Abstractions.Repository;
using GSManager.Core.Abstractions.Services;
using GSManager.Core.Exceptions.Priviledge;
using GSManager.Core.Exceptions.Role;
using GSManager.Core.Extensions;
using GSManager.Core.Filters.Role;
using GSManager.Core.Mappers;
using GSManager.Core.Models.DTOs.Common;
using GSManager.Core.Models.DTOs.Entities;
using GSManager.Core.Models.DTOs.Filters;
using GSManager.Core.Models.DTOs.Requests;
using GSManager.Core.Models.DTOs.Responces;
using GSManager.Core.Models.Entities.Society;
using Microsoft.EntityFrameworkCore;

namespace GSManager.Core.Services;

public class RoleService(
    IUnitOfWork unitOfWork,
    IValidator<RoleDto> validator
    ) : IRoleService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IValidator<RoleDto> _validator = validator;

    public async Task<PagedResultDto<RoleDto>> GetRolesAsync(
        RoleFilterDto filter,
        PagedRequestDto pagedRequest,
        CancellationToken cancellationToken)
    {
        var query = _unitOfWork.Roles.GetQueryable();

        var pipeline = RoleFilterPipeline.Create();
        query = pipeline.Execute(query, filter);

        var pagedRoleResult = await query.ToPagedResultDtoAsync(
            pagedRequest.Page,
            pagedRequest.PageSize,
            RoleMapper.ToDto,
            r => r.Name,
            cancellationToken);

        return pagedRoleResult;
    }

    public async Task<ICollection<SelectListItemDto>> GetRoleSelectListAsync(CancellationToken cancellationToken)
    {
        var roleQuery = _unitOfWork.Roles.GetQueryable();

        return await roleQuery.Select(r =>
            new SelectListItemDto
            {
                Id = r.Id.ToString(),
                Label = r.Name
            }).ToListAsync(cancellationToken) ?? [];
    }

    public async Task<RoleDto> GetRoleByIdAsync(Guid roleId, CancellationToken cancellationToken)
    {
        var role = await _unitOfWork.Roles.GetAsync(
            r => r.Id == roleId,
            cancellationToken
            ) ?? throw new RoleNotFoundException(roleId);

        return RoleMapper.ToDto(role);
    }

    public async Task<RoleDto> AddRoleAsync(RoleDto roleDto, CancellationToken cancellationToken)
    {
        await ValidateRoleAsync(roleDto, cancellationToken);

        var priviledge = await ResolvePriviledgeAsync(roleDto.PriviledgeId, cancellationToken);

        roleDto.Id = null;
        var role = RoleMapper.ToEntity(roleDto, priviledge);

        _unitOfWork.Roles.Add(role);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        roleDto.Id = role.Id;
        return roleDto;
    }

    public async Task<RoleDto> UpdateRoleAsync(Guid roleId, RoleDto roleDto, CancellationToken cancellationToken)
    {
        await ValidateRoleAsync(roleDto, cancellationToken);

        var role = await _unitOfWork.Roles.GetAsync(
            r => r.Id == roleId,
            cancellationToken
            ) ?? throw new RoleNotFoundException(roleId);

        var priviledge = await ResolvePriviledgeAsync(roleDto.PriviledgeId, cancellationToken);

        RoleMapper.UpdateEntity(role, roleDto, priviledge);
        _unitOfWork.Roles.Update(role);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        roleDto.Id = role.Id;
        return roleDto;
    }

    public async Task DeleteRoleAsync(Guid roleId, CancellationToken cancellationToken)
    {
        var role = await _unitOfWork.Roles.GetAsync(
            r => r.Id == roleId,
            cancellationToken
            ) ?? throw new RoleNotFoundException(roleId);

        _unitOfWork.Roles.Remove(role);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    private async Task ValidateRoleAsync(RoleDto roleDto, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(roleDto, cancellationToken);

        if (!validationResult.IsValid)
        {
            throw new InvalidRoleRequestException(validationResult.ToString());
        }
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
}
