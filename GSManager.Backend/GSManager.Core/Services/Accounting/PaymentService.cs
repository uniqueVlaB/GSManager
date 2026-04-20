using FluentValidation;
using GSManager.Core.Abstractions.Repository;
using GSManager.Core.Abstractions.Services.Accounting;
using GSManager.Core.Exceptions.Accounting.Payment;
using GSManager.Core.Extensions;
using GSManager.Core.Mappers.Accounting;
using GSManager.Core.Models.DTOs.Entities.Accounting;
using GSManager.Core.Models.DTOs.Requests;
using GSManager.Core.Models.DTOs.Responces;
using Microsoft.EntityFrameworkCore;

namespace GSManager.Core.Services.Accounting;

public class PaymentService(
    IUnitOfWork unitOfWork,
    IValidator<PaymentDto> validator
    ) : IPaymentService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IValidator<PaymentDto> _validator = validator;

    public async Task<PagedResultDto<PaymentDto>> GetPaymentsAsync(
        PagedRequestDto pagedRequest,
        CancellationToken cancellationToken)
    {
        var query = _unitOfWork.Payments.GetQueryable().AsNoTracking();

        var pagedMemberResult = await query.ToPagedResultDtoAsync(
            pagedRequest.Page,
            pagedRequest.PageSize,
            PaymentMapper.ToDto,
            p => p.PaymentDate,
            cancellationToken);

        return pagedMemberResult;
    }

    public async Task<PaymentDto> GetPaymentByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var payment = await _unitOfWork.Payments.GetAsNoTrackingAsync(p => p.Id == id, cancellationToken);

        return payment is not null ? PaymentMapper.ToDto(payment) : throw new PaymentNotFoundException(id);
    }

    public async Task<PaymentDto> CreatePaymentAsync(PaymentDto request, CancellationToken cancellationToken)
    {
        await ValidatePaymentDtoAsync(request, cancellationToken);

        var payment = PaymentMapper.ToEntity(request);
        _unitOfWork.Payments.Add(payment);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        request.Id = payment.Id;
        return request;
    }

    public async Task<PaymentDto> UpdatePaymentAsync(Guid id, PaymentDto request, CancellationToken cancellationToken)
    {
        await ValidatePaymentDtoAsync(request, cancellationToken);

        var payment = await _unitOfWork.Payments.GetAsync(p => p.Id == id, cancellationToken) ?? throw new PaymentNotFoundException(id);

        PaymentMapper.UpdateEntity(payment, request);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return PaymentMapper.ToDto(payment);
    }

    public async Task DeletePaymentAsync(Guid id, CancellationToken cancellationToken)
    {
        var payment = await _unitOfWork.Payments.GetAsync(p => p.Id == id, cancellationToken) ?? throw new PaymentNotFoundException(id);

        _unitOfWork.Payments.Remove(payment);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    private async Task ValidatePaymentDtoAsync(PaymentDto dto, CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(dto, cancellationToken);

        if (!validationResult.IsValid)
        {
            throw new InvalidPaymentRequestException(validationResult.ToString());
        }
    }
}
