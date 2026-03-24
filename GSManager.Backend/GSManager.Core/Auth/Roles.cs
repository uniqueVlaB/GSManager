namespace GSManager.Core.Auth;

public static class Roles
{
    public const string Admin = "admin";
    public const string Member = "member";

    public static IEnumerable<string> GetAllRoles()
    {
        yield return Admin;
        yield return Member;
    }
}
