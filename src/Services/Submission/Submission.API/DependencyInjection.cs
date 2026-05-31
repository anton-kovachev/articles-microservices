using Articles.Security;
using Auth.Grpc;
using Blocks.AspNetCore.Grpc;
using Blocks.AspNetCore.Providers;
using Blocks.Core;
using Blocks.Core.Security;
using Blocks.Messaging;
using FileStorage.MongoGridFS;
using Journals.Grpc;
using Microsoft.AspNetCore.Http.Json;
using System.Text.Json.Serialization;

namespace Submission.API
{
    public static class DependencyInjection
    {
        public static IServiceCollection ConfigureApiOptions(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddAndValiadateOptions<RabbitMqOptions>(configuration)
                .AddAndValiadateOptions<JwtOptions>(configuration)
                .Configure<JsonOptions>(opts =>
                {
                    opts.SerializerOptions.PropertyNameCaseInsensitive = true;
                    opts.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
                })
               ;

            return services;
        }

        public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddMemoryCache()             //Basic Caching
                .AddEndpointsApiExplorer()    //Minimal API docs (Swagger)
                .AddSwaggerGen();             //Swagger setup

            services.AddScoped<IClaimsProvider, HttpContextProvider>()
                .AddScoped<IHttpContextAccessor, HttpContextAccessor>()
                .AddScoped<HttpContextAccessor>();

            services.AddMongoFileStorageAsSingleton(configuration);

            var grpcOptions = configuration.GetSectionByTypeName<GrpcServiceOptions>();
            //client
            services.AddCodeFirstGrpcClient<IPersonService>(grpcOptions, "Person");
            services.AddCodeFirstGrpcClient<IJournalService>(grpcOptions, "Journal");

            return services;
        }
    }
}
