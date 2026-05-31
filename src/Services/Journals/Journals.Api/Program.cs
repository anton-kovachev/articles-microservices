using Blocks.FastEndpoints;
using FastEndpoints;
using FastEndpoints.Swagger;
using Journals.Api;
using Journals.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureApiOptions(builder.Configuration);

builder.Services
    .AddApiServices(builder.Configuration)
    .AddPersistenceServices(builder.Configuration);

var app = builder.Build();

app.UseSwagger()
   .UseSwaggerUI()
   .UseHttpsRedirection()
   .UseRouting()
   .UseFastEndpoints(config =>
   {
       config.Endpoints.Configurator = ed =>
       {
           ed.PreProcessor<AssignUserIdPreProcessor>(Order.Before);
       };
   })
   .UseSwaggerGen();

app.Run();
