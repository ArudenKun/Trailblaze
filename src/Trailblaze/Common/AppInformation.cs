namespace Trailblaze.Common;

public static class AppInformation
{
    public const string Name = nameof(Trailblaze);

    public static DateTime Born =>
        DateTime.SpecifyKind(new DateTime(2020, 4, 6, 20, 33, 14), DateTimeKind.Utc);
}
