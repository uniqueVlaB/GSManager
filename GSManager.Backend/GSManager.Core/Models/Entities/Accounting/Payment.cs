using GSManager.Core.Models.Entities.Society;

namespace GSManager.Core.Models.Entities.Accounting;

public class Payment
{
    public Guid Id { get; set; }
    public Guid MemberId { get; set; }
    public PaymentType PaymentType { get; set; }
    public decimal Amount { get; set; }
    public DateTime PaymentDate { get; set; }
    public string? Description { get; set; }

    // Navigation properties
    public Member? Member { get; set; }
}
