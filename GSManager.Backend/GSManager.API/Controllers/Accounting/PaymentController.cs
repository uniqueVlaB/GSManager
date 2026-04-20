using GSManager.Core.Abstractions.Services.Accounting;
using GSManager.Core.Auth;
using GSManager.Core.Models.DTOs.Entities.Accounting;
using GSManager.Core.Models.DTOs.Requests;
using GSManager.Core.Models.DTOs.Responces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GSManager.API.Controllers.Accounting;

[Route("api/payments")]
[ApiController]
//[Authorize]
[Tags("Payments")]
public class PaymentController(IPaymentService paymentService) : ControllerBase
{
    private readonly IPaymentService _paymentService = paymentService;

    [HttpGet]
    //[Authorize(Policy = Permissions.ViewPayments)]
    [EndpointSummary("Get payments (paged)")]
    [ProducesResponseType<PagedResultDto<PaymentDto>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPaymentsAsync(
        [FromQuery] PagedRequestDto pagedRequest,
        CancellationToken cancellationToken = default)
    {
        var pagedResult = await _paymentService.GetPaymentsAsync(pagedRequest, cancellationToken);
        return Ok(pagedResult);
    }

    [HttpGet("{id}")]
    //[Authorize(Policy = Permissions.ViewPayments)]
    [EndpointSummary("Get payment by ID")]
    [ProducesResponseType<PaymentDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetPaymentByIdAsync(
        [FromRoute] Guid id,
        CancellationToken cancellationToken = default)
    {
        var payment = await _paymentService.GetPaymentByIdAsync(id, cancellationToken);
        return Ok(payment);
    }

    [HttpPost]
    //[Authorize(Policy = Permissions.CreatePayments)]
    [EndpointSummary("Create a new payment")]
    [ProducesResponseType<PaymentDto>(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreatePaymentAsync(
        [FromBody] PaymentDto request,
        CancellationToken cancellationToken = default)
    {
        var createdPayment = await _paymentService.CreatePaymentAsync(request, cancellationToken);

        return CreatedAtAction(nameof(GetPaymentByIdAsync), new { id = createdPayment.Id }, createdPayment);
    }

    [HttpPut("{id}")]
    //[Authorize(Policy = Permissions.EditPayments)]
    [EndpointSummary("Update an existing payment by ID")]
    [ProducesResponseType<PaymentDto>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdatePaymentAsync(
        [FromRoute] Guid id,
        [FromBody] PaymentDto request,
        CancellationToken cancellationToken = default)
    {
        var updatedPayment = await _paymentService.UpdatePaymentAsync(id, request, cancellationToken);
        return Ok(updatedPayment);
    }

    [HttpDelete("{id}")]
    //[Authorize(Policy = Permissions.DeletePayments)]
    [EndpointSummary("Delete a payment by ID")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeletePaymentAsync(
        [FromRoute] Guid id,
        CancellationToken cancellationToken = default)
    {
        await _paymentService.DeletePaymentAsync(id, cancellationToken);
        return NoContent();
    }
}
