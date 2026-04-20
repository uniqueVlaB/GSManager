using FluentValidation;
using GSManager.Core.Models.DTOs.Entities.Accounting;
using GSManager.Core.Models.Entities.Accounting;

namespace GSManager.Core.FluentValidation.Accounting;

internal sealed class PaymentDtoValidator : AbstractValidator<PaymentDto>
{
    public PaymentDtoValidator()
    {
        When(dto => dto.Id != Guid.Empty, () =>
        RuleFor(dto => dto.Id)
            .NotEqual(Guid.Empty).WithMessage("Id must be a valid GUID."));

        RuleFor(dto => dto.MemberId)
            .NotNull().WithMessage("MemberId is required.");

        RuleFor(dto => dto.PaymentDate)
            .NotNull().WithMessage("PaymentDate is required.");

        RuleFor(dto => dto.PaymentType)
            .NotEmpty().WithMessage("PaymentType is required.")
            .IsEnumName(typeof(PaymentType), caseSensitive: false).WithMessage("Invalid PaymentType.");

        RuleFor(dto => dto.Amount)
            .NotNull().WithMessage("Amount is required.")
            .GreaterThan(0).WithMessage("Amount must be greater than zero.");
    }
}

