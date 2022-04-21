namespace AppointmentJournal.AppCore;

public class SqlOptions
{
    public const string SectionName = "Sql";

    public string? AppConnectionString { get; init; }

    public string? IdentityConnectionString { get; init; }
}