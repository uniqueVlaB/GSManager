using GSManager.Core.Abstractions.Repository.Accounting;
using GSManager.Core.Models.Entities.Accounting;
using GSManager.Infrastructure.SQL.Database;

namespace GSManager.Infrastructure.SQL.Repository.Accounting;

public class PaymentRepository(ApplicationDbContext db) : Repository<Payment>(db), IPaymentRepository
{
    private readonly ApplicationDbContext _db = db;

    public void Update(Payment payment)
    {
        _db.Update(payment);
    }
}
