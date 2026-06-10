using Articles.Security;
using Auth.Grpc;
using Blocks.AspNetCore.Grpc;
using Blocks.AspNetCore.Providers;
using Blocks.Core;
using Blocks.Core.Security;
using Blocks.Messaging;
using Carter;
using EmailService.Contract;
using EmailService.Smtp;
using FileStorage.MongoGridFS;
using Microsoft.AspNetCore.Http.Json;
using Review.API.FileStorage;
using Review.Application.Options;
using System.Text.Json.Serialization;

namespace Review.API
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddMemoryCache()
                .AddCarter()
                .AddHttpContextAccessor()
                .AddEndpointsApiExplorer()
                .AddSwaggerGen()
                .AddJwtAuthentication(configuration)
                .AddAuthorization()
                .AddFileServiceFactory();

            services.AddScoped<IClaimsProvider, HttpContextProvider>()
                .AddScoped<HttpContextAccessor>();

            services.AddMongoFileStorageAsSingleton(configuration);
            services.AddMongoFileStoregeAsScoped<SubmissionFileStorageOptions>(configuration);

            services.AddSmtpMailService(configuration);

            //grpc services
            var grpcOptions = configuration.GetSectionByTypeName<GrpcServiceOptions>();
            services.AddCodeFirstGrpcClient<IPersonService>(grpcOptions, "Person");

            return services;
        }

        public static IServiceCollection ConfigureApiOptions(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddAndValiadateOptions<AppUrlsOptions>(configuration)
                .AddAndValiadateOptions<RabbitMqOptions>(configuration)
                    .Configure<JsonOptions>(opts =>
                    {
                        opts.SerializerOptions.PropertyNameCaseInsensitive = true;
                        opts.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
                    });

            return services;
        }
    }
}
