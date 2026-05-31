using Carter;
using Review.API;
using Review.Application;
using Review.Persistence;

var builder = WebApplication.CreateBuilder(args);

#region Add Services
builder.Services.ConfigureApiOptions(builder.Configuration);

builder.Services
    .AddApiServices(builder.Configuration)
    .AddApplicationServices(builder.Configuration)
    .AddPersistenceServices(builder.Configuration);

#endregion

var app = builder.Build();

#region Use Services
app
    .UseSwagger()
    .UseSwaggerUI()
    .UseRouting()
    .UseAuthorization()
    .UseAuthentication();

app.MapCarter();
//TODO: Create the first migration
if (app.Environment.IsDevelopment())
{
    //TODO: Seed test data
}
#endregion

app.Run();