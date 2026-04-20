using FluentValidation;
using GSManager.Core.Models.DTOs.Requests;

namespace GSManager.Core.FluentValidation.Auth;

internal sealed class CreateUserDtoValidator : AbstractValidator<CreateUserRequestDto>
{
    public CreateUserDtoValidator()
    {
        RuleFor(dto => dto.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email format.");

        RuleFor(dto => dto.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
            .Matches(@"[0-9]").WithMessage("Password must contain at least one digit.")
            .Matches(@"[a-z]").WithMessage("Password must contain at least one lowercase letter.")
            .Matches(@"[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
            .Matches(@"[^a-zA-Z0-9]").WithMessage("Password must contain at least one non-alphanumeric character.");

        RuleFor(dto => dto.Username)
            .NotEmpty().WithMessage("Username is required.")
            .MaximumLength(50).WithMessage("Username cannot exceed 50 characters.");

        When(dto => !string.IsNullOrEmpty(dto.AvatarUrl), () =>
        RuleFor(dto => dto.AvatarUrl)
            .Must(url => Uri.IsWellFormedUriString(url, UriKind.Absolute))
            .WithMessage("Invalid avatar URL format."));
    }
}

