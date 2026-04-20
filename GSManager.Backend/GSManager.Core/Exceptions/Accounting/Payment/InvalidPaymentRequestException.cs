using System.Diagnostics.CodeAnalysis;

namespace GSManager.Core.Exceptions.Accounting.Payment;

[ExcludeFromCodeCoverage]
public class InvalidPaymentRequestException : GSManagerInvalidRequestException
{
    public InvalidPaymentRequestException()
        : base("Invalid payment request.")
    {
    }

    public InvalidPaymentRequestException(string message)
        : base(message)
    {
    }
}
