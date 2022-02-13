# labs-healthz
Health checks lab in ASP.NET Core. Uses [AspNetCore.Diagnostics.HealthChecks](https://github.com/Xabaril/AspNetCore.Diagnostics.HealthChecks) and the `UI` but without the need of the `AspNetCore.HealthChecks.UI.Client` to get rid of unwanted depencencies it carries.

Three APIs:
- **HealthyApi:**
Uses the `AspNetCore.HealthChecks.UI.Client.UIResponseWriter` (causing unwanted dependencies on e.g. `Microsoft.EntityFrameworkCore`)

- **HealthyApi2:**
Uses custom Health report model, rewritten in UI `HealthReportInfoHttpClientHandler` using the `UIResponseWriter`.

- **HealthyApi3:**
Uses custom Health report model as is.

- **HealthyUi:** Hooks in the `AspNetCore.HealthChecks.UI` using in memory storage.

## Lab resources
The `docker-compose-yml` file as well as APIs, need an `.env` file in the repo root.

```
touch .env
```

```
mongodb_user = foo
mongodb_pass = bar
mongodb_connection = mongodb://foo:bar@localhost:27017
mssql_pass = bar
mssql_connection = Server=.,1433;User Id=foo;Password=bar;
rabbitmq_user = foo
rabbitmq_pass = bar
rabbitmq_connection = amqp://foo:bar@localhost:5672
```

Then you can just: `docker compose up -d` and start the projects. After that stop indivudial containers and look how the failures start getting reported in the UI.
