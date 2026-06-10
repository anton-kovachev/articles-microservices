using ArticleHub.API;
using ArticleHub.Persistence;
using Blocks.AspNetCore.Filters;
using Carter;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureApiOptions(builder.Configuration);

builder.Services
    .AddApiAndApplicationServices(builder.Configuration)
    .AddPersistenceServices(builder.Configuration);


var app = builder.Build();

app
    .UseSwagger()
    .UseSwaggerUI()
    .UseRouting()
    .UseAuthentication()
    .UseAuthorization();
    //.UseMiddleware<GlobalExceptionMiddleware>()
    //.UseMiddleware<RequestContextMiddleware>()
    //.UseMiddleware<ResponseTimingMiddleware>();

var api = app.MapGroup("/api").AddEndpointFilter<AssignUserIdFilter>();
api.MapCarter();

//app.Migrate<ArticleHubDbContext>();

if (app.Environment.IsDevelopment())
{
    //app.SeedTestData();
}

app.Run();
