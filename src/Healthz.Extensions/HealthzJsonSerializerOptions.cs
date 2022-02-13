using System.Text.Json;
using System.Text.Json.Serialization;

namespace Healthz.Extensions;

public static class HealthzJsonSerializerOptions
{
    private static readonly Lazy<JsonSerializerOptions> Lazy;
    public static Func<JsonSerializerOptions, JsonSerializerOptions> Initializer { get; set; } = opts => opts;

    public static JsonSerializerOptions Options => Lazy.Value;

    static HealthzJsonSerializerOptions()
    {
        Lazy = new Lazy<JsonSerializerOptions>(() =>
        {
            var defaultOptions = CreateDefaultOptions();

            return Initializer?.Invoke(defaultOptions) ?? defaultOptions;
        });
    }

    private static JsonSerializerOptions CreateDefaultOptions() => new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        Converters = {new JsonStringEnumConverter()}
    };
}
