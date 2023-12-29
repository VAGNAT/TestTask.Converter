using Ocelot.DependencyInjection;
using Ocelot.Errors.Middleware;
using Ocelot.Middleware;
using Plumsail.Converter;
using Plumsail.Converter.Middleware;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

var policy = "OcelotPolicy";

builder.Services.AddCors(options =>
{
    options.AddPolicy(policy,
        builder => builder.WithOrigins("https://localhost:3000")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials()); ;
});

builder.Configuration.SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("configuration.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"configuration.Development.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables();

builder.Services.AddOcelot(builder.Configuration);

builder.WebHost.ConfigureKestrel(srvOpt => srvOpt.Limits.MaxRequestBodySize = null);

Registrar.ConfigureLogging();
builder.Host.UseSerilog();

var app = builder.Build();

app.UseCors(policy);

app.UseLoggingMiddleware();

await app.UseOcelot();

app.Run();
