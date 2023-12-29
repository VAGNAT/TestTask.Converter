using MassTransit;
using Plumsail.FileService;
using Plumsail.FileService.Consumers;
using Serilog;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddServices(builder.Configuration);

Registrar.ConfigureLogging();
builder.Host.UseSerilog();

var app = builder.Build();

app.Run();
