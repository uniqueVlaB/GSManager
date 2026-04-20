namespace GSManager.Core.Models.DTOs.Entities.Accounting;

public class PaymentDto
{
    public Guid? Id { get; set; }
    public Guid? MemberId { get; set; }
    public string? PaymentType { get; set; } = string.Empty;
    public decimal? Amount { get; set; }
    public DateTime? PaymentDate { get; set; }
    public string? Description { get; set; }
}
