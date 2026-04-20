using GSManager.Core.Models.DTOs.Entities.Accounting;
using GSManager.Core.Models.Entities.Accounting;

namespace GSManager.Core.Mappers.Accounting;

public static class PaymentMapper
{
    public static PaymentDto ToDto(Payment payment)
    {
        return new PaymentDto
        {
            Id = payment.Id,
            MemberId = payment.MemberId,
            PaymentDate = payment.PaymentDate,
            PaymentType = payment.PaymentType.ToString(),
            Amount = payment.Amount,
            Description = payment.Description
        };
    }

    public static Payment ToEntity(PaymentDto paymentDto)
    {
        return new Payment
        {
            Id = paymentDto.Id ?? Guid.NewGuid(),
            MemberId = paymentDto.MemberId!.Value,
            PaymentDate = paymentDto.PaymentDate!.Value,
            PaymentType = Enum.Parse<PaymentType>(paymentDto.PaymentType!, ignoreCase: true),
            Amount = paymentDto.Amount!.Value,
            Description = paymentDto.Description
        };
    }

    public static void UpdateEntity(Payment payment, PaymentDto paymentDto)
    {
        payment.MemberId = paymentDto.MemberId!.Value;
        payment.PaymentDate = paymentDto.PaymentDate!.Value;
        payment.PaymentType = Enum.Parse<PaymentType>(paymentDto.PaymentType!, ignoreCase: true);
        payment.Amount = paymentDto.Amount!.Value;
        payment.Description = paymentDto.Description;
    }
}
