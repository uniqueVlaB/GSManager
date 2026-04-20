using GSManager.Core.Models.DTOs.Entities.Accounting;
using GSManager.Core.Models.DTOs.Requests;
using GSManager.Core.Models.DTOs.Responces;

namespace GSManager.Core.Abstractions.Services.Accounting;

public interface IPaymentService
{
    Task<PagedResultDto<PaymentDto>> GetPaymentsAsync(PagedRequestDto pagedRequest, CancellationToken cancellationToken);
    Task<PaymentDto> GetPaymentByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<PaymentDto> CreatePaymentAsync(PaymentDto request, CancellationToken cancellationToken);
    Task<PaymentDto> UpdatePaymentAsync(Guid id, PaymentDto request, CancellationToken cancellationToken);
    Task DeletePaymentAsync(Guid id, CancellationToken cancellationToken);
}
