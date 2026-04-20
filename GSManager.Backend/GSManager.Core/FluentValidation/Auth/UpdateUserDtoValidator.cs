using FluentValidation;
using GSManager.Core.Models.DTOs.Requests;

namespace GSManager.Core.FluentValidation.Auth;

internal sealed class UpdateUserDtoValidator : AbstractValidator<UpdateUserRequestDto>
{
    public UpdateUserDtoValidator()
    {
        When(dto => !string.IsNullOrEmpty(dto.Email), () =>
        RuleFor(dto => dto.Email)
                .EmailAddress().WithMessage("Invalid email format."));

        When(dto => !string.IsNullOrEmpty(dto.Password), () =>
        RuleFor(dto => dto.Password)
                .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
                .Matches(@"[0-9]").WithMessage("Password must contain at least one digit.")
                .Matches(@"[a-z]").WithMessage("Password must contain at least one lowercase letter.")
                .Matches(@"[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
                .Matches(@"[^a-zA-Z0-9]").WithMessage("Password must contain at least one non-alphanumeric character."));

        When(dto => !string.IsNullOrEmpty(dto.Username), () =>
        RuleFor(dto => dto.Username)
                .MaximumLength(50).WithMessage("Username cannot exceed 50 characters."));
    }
}

