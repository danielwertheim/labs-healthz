using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Healthz.Contracts;

public record HealthzDetailedInfo(
    Dictionary<string, HealthzDetailedEntryInfo> Entries,
    TimeSpan TotalDuration,
    HealthStatus Status)
{
    public static HealthzDetailedInfo From(HealthReport src)
        => new(src.Entries.ToDictionary(e => e.Key, e => HealthzDetailedEntryInfo.From(e.Value)), src.TotalDuration, src.Status);

    public HealthReport Map()
        => new(
            Entries.ToDictionary(e => e.Key, e => e.Value.Map()),
            Status,
            TotalDuration);
}

public record HealthzDetailedEntryInfo(
    HealthStatus Status,
    TimeSpan Duration,
    Dictionary<string, object> Data,
    IEnumerable<string> Tags,
    string? Description,
    string? Exception)
{
    public static HealthzDetailedEntryInfo From(HealthReportEntry src)
        => new(
            src.Status,
            src.Duration,
            src.Data.ToDictionary(kv => kv.Key, kv => kv.Value),
            src.Tags,
            src.Description ?? src.Exception?.Message,
            src.Exception?.Message);

    public HealthReportEntry Map()
        => new(
            Status,
            Description,
            Duration,
            Exception != null ? new Exception(Exception) : null,
            Data,
            Tags);
}
