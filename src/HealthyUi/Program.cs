using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;
using HealthChecks.UI.Core;
using Healthz.Contracts;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddHealthChecksUI(setup =>
    {
        setup.DisableDatabaseMigrations();

        setup.UseApiEndpointHttpMessageHandler(_ => new HealthReportInfoHttpClientHandler());
    })
    .AddInMemoryStorage();

var app = builder.Build();

app
    .UseRouting()
    .UseEndpoints(cfg => cfg.MapHealthChecksUI());

app.Run();

/// <summary>
/// Maps from <see cref="HealthzDetailedInfo"/> to <see cref="UIHealthReport"/>.
/// </summary>
public class HealthReportInfoHttpClientHandler : HttpClientHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        //For lab only, we do not want to affect HealthyApi
        if(request.RequestUri == new Uri("http://localhost:5201/healthz"))
            return await base.SendAsync(request, cancellationToken);

        //For lab only, we do not want to affect HealthyApi3
        if(request.RequestUri == new Uri("http://localhost:5203/healthz"))
            return await base.SendAsync(request, cancellationToken);

        //For lab only, we only want to affect HealthyApi2
        var org = await base.SendAsync(request, cancellationToken);
        if (!org.IsSuccessStatusCode && org.StatusCode != HttpStatusCode.ServiceUnavailable)
            return org;

        var healthReportInfo = await org.Content.ReadFromJsonAsync<HealthzDetailedInfo>(
            Healthz.Extensions.HealthzJsonSerializerOptions.Options,
            cancellationToken);
        var uiReport = UIHealthReport.CreateFrom(healthReportInfo!.Map());
        org.Content = JsonContent.Create(uiReport);

        return org;
    }
}
