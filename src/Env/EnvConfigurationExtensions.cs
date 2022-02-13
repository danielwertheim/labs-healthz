using Microsoft.Extensions.Configuration;

namespace Env;

public static class EnvConfigurationExtensions
{
    public static IConfigurationBuilder AddOptionalEnvFile(this IConfigurationBuilder builder, string path)
    {
        if (!File.Exists(path))
            return builder;

        var envKeyValues =
            File
                .ReadAllLines(path)
                .Select(l =>
                {
                    var parts = l.Split("=");
                    return new KeyValuePair<string, string>(parts[0].Trim(), string.Join("=", parts.Skip(1)).Trim());
                })
                .ToList();

        if (envKeyValues.Any())
            builder.AddInMemoryCollection(envKeyValues);

        return builder;
    }
}
