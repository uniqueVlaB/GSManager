using GSManager.Core.Abstractions.Repository;
using GSManager.Core.Abstractions.Repository.Society;
using GSManager.Infrastructure.SQL.Database;
using Microsoft.AspNetCore.Identity;

namespace GSManager.Infrastructure.SQL.Repository.Society;

public class IdentityUserRoleRepository(ApplicationDbContext db) : Repository<IdentityUserRole<Guid>>(db), IIdentityUserRoleRepository
{
}

