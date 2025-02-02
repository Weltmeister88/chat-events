namespace ChatEvents.ConfigurationOptions;

public record DatabaseOptions
{
    public static string ConfigurationKey => "Database";
    public required string DatabaseName { get; init; }
}