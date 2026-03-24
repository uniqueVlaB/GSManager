namespace GSManager.Core.Auth;

public static class Permissions
{
    public const string FullAccess = "full_access";

    public const string ViewMembers = "members:view";
    public const string AddMembers = "members:add";
    public const string EditMembers = "members:edit";
    public const string DeleteMembers = "members:delete";

    public const string ViewPlots = "plots:view";
    public const string AddPlots = "plots:add";
    public const string EditPlots = "plots:edit";
    public const string DeletePlots = "plots:delete";

    public const string ViewUsers = "users:view";
    public const string AddUsers = "users:add";
    public const string EditUsers = "users:edit";
    public const string DeleteUsers = "users:delete";

    public static IEnumerable<string> GetAllPermissions()
    {
        yield return ViewMembers;
        yield return AddMembers;
        yield return EditMembers;
        yield return DeleteMembers;

        yield return ViewPlots;
        yield return AddPlots;
        yield return EditPlots;
        yield return DeletePlots;

        yield return ViewUsers;
        yield return AddUsers;
        yield return EditUsers;
        yield return DeleteUsers;
    }
}
