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

    public const string ViewRoles = "roles:view";
    public const string AddRoles = "roles:add";
    public const string EditRoles = "roles:edit";
    public const string DeleteRoles = "roles:delete";

    public const string ViewPriviledges = "priviledges:view";
    public const string AddPriviledges = "priviledges:add";
    public const string EditPriviledges = "priviledges:edit";
    public const string DeletePriviledges = "priviledges:delete";

    public const string ViewElectricityMeters = "electricity_meters:view";
    public const string AddElectricityMeters = "electricity_meters:add";
    public const string EditElectricityMeters = "electricity_meters:edit";
    public const string DeleteElectricityMeters = "electricity_meters:delete";

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

        yield return ViewRoles;
        yield return AddRoles;
        yield return EditRoles;
        yield return DeleteRoles;

        yield return ViewPriviledges;
        yield return AddPriviledges;
        yield return EditPriviledges;
        yield return DeletePriviledges;

        yield return ViewElectricityMeters;
        yield return AddElectricityMeters;
        yield return EditElectricityMeters;
        yield return DeleteElectricityMeters;
    }
}
