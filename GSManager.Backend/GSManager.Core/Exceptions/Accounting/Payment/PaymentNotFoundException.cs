using System.Diagnostics.CodeAnalysis;

namespace GSManager.Core.Exceptions.Accounting.Payment;

[ExcludeFromCodeCoverage]
public class PaymentNotFoundException : GSManagerNotFoundException
{
    public PaymentNotFoundException(Guid id)
        : base($"Payment with id '{id}' not found.")
    {
    }

    public PaymentNotFoundException()
        : base("Payment not found.")
    {
    }
}
