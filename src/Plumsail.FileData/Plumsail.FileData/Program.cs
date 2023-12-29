using Plumsail.FileData;
using Plumsail.FileData.Infrastructure;
using Plumsail.FileData.Middleware;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddServices(builder.Configuration);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.WebHost.ConfigureKestrel(srvOpt => srvOpt.Limits.MaxRequestBodySize = null);

Registrar.ConfigureLogging();
builder.Host.UseSerilog();

var app = builder.Build();


app.UseHttpsRedirection();

app.UseExceptionHandlerMiddleware();
app.UseCheckCookiesMiddleware();

app.MapControllers();

app.Services.InitializeInfrastructureServices();

app.Run();
