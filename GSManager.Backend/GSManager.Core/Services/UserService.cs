using System.Security.Claims;
using FluentValidation;
using GSManager.Core.Abstractions.Mailer;
using GSManager.Core.Abstractions.Repository;
using GSManager.Core.Abstractions.Services;
using GSManager.Core.Auth;
using GSManager.Core.Exceptions;
using GSManager.Core.Exceptions.Auth;
using GSManager.Core.Mappers;
using GSManager.Core.Models.DTOs.Requests;
using GSManager.Core.Models.DTOs.Responces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GSManager.Core.Services;

public class UserService(
    UserManager<ApplicationUser> userManager,
    RoleManager<IdentityRole<Guid>> roleManager,
    IUnitOfWork unitOfWork,
    IHttpContextAccessor httpContextAccessor,
    IValidator<CreateUserRequestDto> createUserRequestDtoValidator,
    IValidator<UpdateUserRequestDto> updateUserRequestDtoValidator,
    IMailer mailer) : IUserService
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly RoleManager<IdentityRole<Guid>> _roleManager = roleManager;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
    private readonly IValidator<CreateUserRequestDto> _createUserRequestDtoValidator = createUserRequestDtoValidator;
    private readonly IValidator<UpdateUserRequestDto> _updateUserRequestDtoValidator = updateUserRequestDtoValidator;
    private readonly IMailer _mailer = mailer;

    public async Task<UserResponseDto> CreateUserAsync(CreateUserRequestDto createUserDto, CancellationToken cancellationToken)
    {
        await ValidateCreateUserRequestDtoAsync(createUserDto, cancellationToken);

        try
        {
            await _unitOfWork.BeginTransactionAsync(cancellationToken);

            var newUser = await CreateIdentityUserAsync(createUserDto);
            var assignedRoles = await AssignRolesToUserAsync(newUser, createUserDto.RoleIds, cancellationToken);
            await AssignPermissionsToUserAsync(newUser, createUserDto.Permissions);

            await _unitOfWork.CommitAsync(cancellationToken);

            var confirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);
            _mailer.SendEmailConfirmation(newUser.Email!, newUser.UserName!, newUser.Id, confirmationToken);

            return UserMapper.ToUserResponseDto(
                newUser,
                assignedRoles?.Select(r => r.Name!).ToList(),
                createUserDto.Permissions);
        }
        catch
        {
            await _unitOfWork.RollbackAsync(cancellationToken);
            throw;
        }
    }

    public async Task<UserResponseDto> UpdateUserAsync(Guid userId, UpdateUserRequestDto updateUserDto, CancellationToken cancellationToken)
    {
        await ValidateUpdateUserRequestDtoAsync(updateUserDto, cancellationToken);

        var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == userId, cancellationToken)
            ?? throw new UserNotFoundException(userId);

        try
        {
            await _unitOfWork.BeginTransactionAsync(cancellationToken);

            await ApplyProfileChangesAsync(user, updateUserDto);
            await ResetPasswordIfProvidedAsync(user, updateUserDto.Password);
            await ReplaceRolesIfProvidedAsync(user, updateUserDto.RoleIds, cancellationToken);
            await ReplacePermissionsIfProvidedAsync(user, updateUserDto.Permissions);

            await _unitOfWork.CommitAsync(cancellationToken);

            return await CreateUserResponseDtoFromUserAsync(user);
        }
        catch
        {
            await _unitOfWork.RollbackAsync(cancellationToken);
            throw;
        }
    }

    private async Task ApplyProfileChangesAsync(ApplicationUser user, UpdateUserRequestDto dto)
    {
        UpdateNotNullProperties(user, dto);

        var result = await _userManager.UpdateAsync(user);
        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            throw new GSManagerException($"Failed to update user: {errors}");
        }
    }

    private async Task ResetPasswordIfProvidedAsync(ApplicationUser user, string? password)
    {
        if (string.IsNullOrEmpty(password))
        {
            return;
        }

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        var result = await _userManager.ResetPasswordAsync(user, token, password);
        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            throw new GSManagerException($"Failed to update password: {errors}");
        }
    }

    private async Task ReplaceRolesIfProvidedAsync(ApplicationUser user, ICollection<Guid>? roleIds, CancellationToken cancellationToken)
    {
        if (roleIds == null || roleIds.Count == 0)
        {
            return;
        }

        var currentRoles = await _userManager.GetRolesAsync(user);
        await _userManager.RemoveFromRolesAsync(user, currentRoles);
        await AssignRolesToUserAsync(user, roleIds, cancellationToken);
    }

    private async Task ReplacePermissionsIfProvidedAsync(ApplicationUser user, ICollection<string>? permissions)
    {
        if (permissions == null || permissions.Count == 0)
        {
            return;
        }

        var currentClaims = (await _userManager.GetClaimsAsync(user))
            .Where(c => c.Type == CustomClaimTypes.Permission)
            .ToList();
        await _userManager.RemoveClaimsAsync(user, currentClaims);
        await AssignPermissionsToUserAsync(user, permissions);
    }

    public async Task DeleteUserAsync(Guid userId, CancellationToken cancellationToken)
    {
        var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == userId, cancellationToken)
             ?? throw new UserNotFoundException(userId);
        await _userManager.DeleteAsync(user);
    }

    public async Task<UserResponseDto> GetCurrentUserDtoAsync(CancellationToken cancellationToken)
    {
        var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext!.User)
            ?? throw new UserNotFoundException();

        return await CreateUserResponseDtoFromUserAsync(user);
    }

    public async Task<UserResponseDto> GetUserByIdAsync(Guid userId, CancellationToken cancellationToken)
    {
        var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == userId, cancellationToken)
            ?? throw new UserNotFoundException(userId);

        return await CreateUserResponseDtoFromUserAsync(user);
    }

    public async Task<PagedResultDto<UserResponseDto>> GetAllUsersAsync(PagedRequestDto pagedRequest, CancellationToken cancellationToken)
    {
        var pageNumber = Math.Max(pagedRequest.Page, 1);
        var pageSize = Math.Clamp(pagedRequest.PageSize, 1, 100);

        var query = _userManager.Users.OrderBy(u => u.Id);
        var totalCount = await query.CountAsync(cancellationToken);

        var users = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        var userIds = users.Select(u => u.Id).ToList();

        var rolesByUserId = await FetchRolesByUserIdsAsync(userIds, cancellationToken);
        var permissionsByUserId = await FetchPermissionsByUserIdsAsync(userIds, cancellationToken);

        var userDtos = users
            .Select(u => UserMapper.ToUserResponseDto(
                u,
                rolesByUserId.GetValueOrDefault(u.Id) ?? [],
                permissionsByUserId.GetValueOrDefault(u.Id) ?? []))
            .ToList();

        return new PagedResultDto<UserResponseDto>
        {
            Items = userDtos,
            TotalCount = totalCount,
            CurrentPage = pageNumber,
            PageSize = pageSize
        };
    }

    private async Task<Dictionary<Guid, IList<string>>> FetchRolesByUserIdsAsync(List<Guid> userIds, CancellationToken cancellationToken)
    {
        var lookup = await (from ur in _unitOfWork.IdentityUserRoles.GetQueryable()
                            join r in _unitOfWork.IdentityRoles.GetQueryable() on ur.RoleId equals r.Id
                            where userIds.Contains(ur.UserId)
                            select new { ur.UserId, RoleName = r.Name! })
            .ToListAsync(cancellationToken);

        return lookup
            .GroupBy(x => x.UserId)
            .ToDictionary(g => g.Key, g => g.Select(x => x.RoleName).ToList() as IList<string>);
    }

    private async Task<Dictionary<Guid, IList<string>>> FetchPermissionsByUserIdsAsync(List<Guid> userIds, CancellationToken cancellationToken)
    {
        var lookup = await _unitOfWork.IdentityUserClaims.GetQueryable()
            .Where(uc => userIds.Contains(uc.UserId) && uc.ClaimType == CustomClaimTypes.Permission)
            .Select(uc => new { uc.UserId, uc.ClaimValue })
            .ToListAsync(cancellationToken);

        return lookup
            .GroupBy(x => x.UserId)
            .ToDictionary(g => g.Key, g => g.Select(x => x.ClaimValue!).ToList() as IList<string>);
    }

    private static void UpdateNotNullProperties(ApplicationUser user, UpdateUserRequestDto dto)
    {
        if (!string.IsNullOrEmpty(dto.Username))
        {
            user.UserName = dto.Username;
        }

        if (!string.IsNullOrEmpty(dto.Email))
        {
            user.Email = dto.Email;
        }

        if (dto.MemberId != null)
        {
            user.MemberId = dto.MemberId;
        }

        if (!string.IsNullOrEmpty(dto.AvatarUrl))
        {
            user.AvatarUrl = dto.AvatarUrl;
        }
    }

    private async Task ValidateCreateUserRequestDtoAsync(CreateUserRequestDto dto, CancellationToken cancellationToken)
    {
        var validationResult = await _createUserRequestDtoValidator.ValidateAsync(dto, cancellationToken);

        if (!validationResult.IsValid)
        {
            throw new InvalidUserRequestException(validationResult.ToString());
        }
    }

    private async Task ValidateUpdateUserRequestDtoAsync(UpdateUserRequestDto dto, CancellationToken cancellationToken)
    {
        var validationResult = await _updateUserRequestDtoValidator.ValidateAsync(dto, cancellationToken);
        if (!validationResult.IsValid)
        {
            throw new InvalidUserRequestException(validationResult.ToString());
        }
    }

    private async Task<ApplicationUser> CreateIdentityUserAsync(CreateUserRequestDto dto)
    {
        var newUser = new ApplicationUser
        {
            Id = Guid.NewGuid(),
            UserName = dto.Username,
            Email = dto.Email,
            MemberId = dto.MemberId,
            AvatarUrl = dto.AvatarUrl
        };

        if (await _userManager.FindByEmailAsync(dto.Email) != null)
        {
            throw new InvalidUserRequestException("A user with the provided email already exists.");
        }

        var result = await _userManager.CreateAsync(newUser, dto.Password);

        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            throw new GSManagerException($"Failed to create user: {errors}");
        }

        return newUser;
    }

    private async Task<List<IdentityRole<Guid>>?> AssignRolesToUserAsync(ApplicationUser user, ICollection<Guid>? roleIds, CancellationToken cancellationToken)
    {
        if (roleIds == null || roleIds.Count == 0)
        {
            return null;
        }

        var existingRoles = await _roleManager.Roles
            .Where(r => roleIds.Contains(r.Id))
            .ToListAsync(cancellationToken);

        if (existingRoles.Count != roleIds.Count)
        {
            var existingRoleIds = existingRoles.Select(r => r.Id);
            var missingRoleIds = roleIds.Where(id => !existingRoleIds.Contains(id));
            throw new IdentityRoleNotFoundException(missingRoleIds);
        }

        foreach (var role in existingRoles)
        {
            var result = await _userManager.AddToRoleAsync(user, role.Name!);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new GSManagerException($"Failed to add user to role '{role.Name}': {errors}");
            }
        }

        return existingRoles;
    }

    private async Task AssignPermissionsToUserAsync(ApplicationUser user, ICollection<string>? permissions)
    {
        if (permissions == null || permissions.Count == 0)
        {
            return;
        }

        var appPermissions = Permissions.GetAllPermissions();

        foreach (var permission in permissions)
        {
            if (!appPermissions.Contains(permission))
            {
                throw new InvalidPermissionException(permission);
            }

            var result = await _userManager.AddClaimAsync(user, new Claim(CustomClaimTypes.Permission, permission));
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new GSManagerException($"Failed to add permission '{permission}' to user: {errors}");
            }
        }
    }

    private async Task<UserResponseDto> CreateUserResponseDtoFromUserAsync(ApplicationUser user)
    {
        var userRoles = await _userManager.GetRolesAsync(user);
        var userClaims = await _userManager.GetClaimsAsync(user);

        var permissions = userClaims.Where(c => c.Type == CustomClaimTypes.Permission).Select(c => c.Value).ToList();

        return UserMapper.ToUserResponseDto(user, userRoles, permissions);
    }
}
