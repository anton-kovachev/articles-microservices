using Blocks.AspNetCore.Filters;
using Blocks.AspNetCore.Middlewares;
using Blocks.EntityFrameworkCore.Extensions;
using Submission.API;
using Submission.API.Endpoints;
using Submission.Application;
using Submission.Persistence;

var builder = WebApplication.CreateBuilder(args);

#region Add Services

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
    .UseMiddleware<GlobalExceptionMiddleware>();

app.MappAllEndpoints();
app.MapGroup("/api").AddEndpointFilter<AssignUserIdFilter>();
app.Migrate<SubmissionDbContext>();

if (app.Environment.IsDevelopment())
{
    //TODO: Seed test data
}
#endregion

app.Run();
