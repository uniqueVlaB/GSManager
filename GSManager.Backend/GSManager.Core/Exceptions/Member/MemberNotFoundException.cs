using System.Diagnostics.CodeAnalysis;

namespace GSManager.Core.Exceptions.Member;

[ExcludeFromCodeCoverage]
public class MemberNotFoundException : GSManagerNotFoundException
{
    public MemberNotFoundException(Guid id)
        : base($"Member with id '{id}' not found.")
    {
    }

    public MemberNotFoundException()
        : base("Member not found.")
    {
    }
}
