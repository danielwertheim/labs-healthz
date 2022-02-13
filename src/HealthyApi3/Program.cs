using Env;
using Healthz.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Host.ConfigureAppConfiguration(config => config
    .AddOptionalEnvFile(Path.Combine(AppContext.BaseDirectory, ".env")));

// Add services to the container.
builder.Services.AddControllers();

builder.Services
    .AddHealthChecks()
    .AddRabbitMQ(new Uri(builder.Configuration["rabbitmq_connection"]), tags: new[] {HealthzTags.Ready})
    .AddMongoDb(builder.Configuration["mongodb_connection"], timeout: TimeSpan.FromSeconds(5), tags: new[] {HealthzTags.Ready})
    .AddSqlServer(builder.Configuration["mssql_connection"], timeout: TimeSpan.FromSeconds(5), tags: new[] {HealthzTags.Ready});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapControllers();

app.MapHealthzChecks();

app.Run();
