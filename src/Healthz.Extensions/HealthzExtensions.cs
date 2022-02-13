using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Healthz.Extensions;

public static class HealthzExtensions
{
    /// <summary>
    /// Maps <![CDATA[/healthz]]> to include all health checks.
    /// </summary>
    /// <param name="endpoints"></param>
    /// <returns></returns>
    public static IEndpointRouteBuilder MapHealthzWithAllChecks(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapHealthChecks("/healthz");

        return endpoints;
    }

    /// <summary>
    /// Maps <![CDATA[/healthz-details]]> to include all health checks with detailed information in the response.
    /// </summary>
    /// <param name="endpoints"></param>
    /// <param name="responseWriter"></param>
    /// <returns></returns>
    public static IEndpointRouteBuilder MapHealthzDetailedWithAllChecks(this IEndpointRouteBuilder endpoints, Func<HttpContext, HealthReport, Task>? responseWriter = null)
    {
        endpoints.MapHealthChecks("/healthz-details", new HealthCheckOptions
        {
            ResponseWriter = responseWriter ?? HealthzResponseWriter.Default
        });

        return endpoints;
    }

    /// <summary>
    /// Maps <![CDATA[/healthz/ready]]> to include all health checks with filter <code>ready</code>.
    /// </summary>
    /// <param name="endpoints"></param>
    /// <returns></returns>
    public static IEndpointRouteBuilder MapHealthzWithReadinessChecks(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapHealthChecks("/healthz/ready", new HealthCheckOptions
        {
            Predicate = healthCheck => healthCheck.Tags.Contains(HealthzTags.Ready)
        });

        return endpoints;
    }

    /// <summary>
    /// Maps <![CDATA[/healthz/live]]> without any health checks.
    /// </summary>
    /// <param name="endpoints"></param>
    /// <returns></returns>
    public static IEndpointRouteBuilder MapHealthzWithLivenessChecks(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapHealthChecks("/healthz/live", new HealthCheckOptions
        {
            Predicate = _ => false
        });

        return endpoints;
    }

    /// <summary>
    /// Maps
    /// - <see cref="MapHealthzWithAllChecks"/>
    /// - <see cref="MapHealthzDetailedWithAllChecks"/>
    /// - <see cref="MapHealthzWithLivenessChecks"/>
    /// - <see cref="MapHealthzWithReadinessChecks"/>
    /// </summary>
    /// <param name="endpoints"></param>
    public static void MapHealthzChecks(this IEndpointRouteBuilder endpoints)
    {
        endpoints
            .MapHealthzWithAllChecks()
            .MapHealthzDetailedWithAllChecks()
            .MapHealthzWithLivenessChecks()
            .MapHealthzWithReadinessChecks();
    }
}
