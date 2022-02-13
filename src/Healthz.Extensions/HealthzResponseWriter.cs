using System;
using System.Threading.Tasks;
using Healthz.Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Healthz.Extensions
{
    internal static class HealthzResponseWriter
    {
        internal static readonly Func<HttpContext, HealthReport, Task> Default =
            (context, report) => context.Response.WriteAsJsonAsync(HealthzDetailedInfo.From(report), HealthzJsonSerializerOptions.Options);
    }
}
