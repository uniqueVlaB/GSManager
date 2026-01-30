namespace GSManager.Core.Constants;

public static class SortingOptions
{
    public const string PlotNumberAsc = "Plot number ascending";
    public const string PlotNumberDesc = "Plot number descending";
    public const string LastNameAsc = "Last name ascending";
    public const string LastNameDesc = "Last name descending";
    public const string FirstNameAsc = "First name ascending";
    public const string FirstNameDesc = "First name descending";
    public const string RoleNameAsc = "Role name ascending";
    public const string RoleNameDesc = "Role name descending";

    public static readonly List<string> Options = [PlotNumberAsc, PlotNumberDesc, LastNameAsc, LastNameDesc, FirstNameAsc, FirstNameDesc];
}
