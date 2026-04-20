using GSManager.Core.Models.Entities.Accounting;

namespace GSManager.Core.Abstractions.Repository.Accounting;

public interface IPaymentRepository : IRepository<Payment>
{
    void Update(Payment payment);
}
