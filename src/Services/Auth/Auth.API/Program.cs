using Auth.API;
using Auth.API.Features.Persons;
using Auth.Application;
using Auth.Persistence;
using Auth.Persistence.Data.Test;
using Blocks.EntityFrameworkCore.Extensions;
using FastEndpoints.Swagger;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureApiOptions(builder.Configuration);
builder.Services
    .AddApplicationServices()
    .AddApiServices(builder.Configuration)
    .AddPersistenceServices(builder.Configuration);

var app = builder.Build();

if (builder.Environment.IsDevelopment())
{
    app.Migrate<AuthDbContext>();
    app.SeedTestData();
}

app
    .UseHttpsRedirection()
    .UseRouting()
    .UseAuthentication()
    .UseAuthentication()
    .UseFastEndpoints()
    .UseSwaggerGen();

app.MapGrpcService<PersonGrpcService>();
app.MapFastEndpoints();
app.Run();
