using FileStorage.Contract;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Blocks.Core;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using Microsoft.Extensions.Options;

namespace FileStorage.MongoGridFS;

public static class FileStorageRegistration
{
    public static IServiceCollection AddMongoFileStorageAsSingleton(this IServiceCollection services, IConfiguration configuration)
    {
        var section = configuration.GetSection(nameof(MongoGridFsFileStorageOptions));

        if (!section.Exists())
            throw new InvalidOperationException($"Configuration section '{section.Key}' is missing.");

        services.AddAndValiadateOptions<MongoGridFsFileStorageOptions>(configuration);

        var options = configuration.GetSectionByTypeName<MongoGridFsFileStorageOptions>();

        services.AddSingleton<IMongoClient>(sp =>
        {
            return new MongoClient(configuration.GetConnectionStringOrThrow(options.ConnectionStringName));
        });

        services.AddSingleton(sp =>
        {
            var mongoClient = sp.GetRequiredService<IMongoClient>();
            return mongoClient.GetDatabase(options.DatabaseName);
        });

        services.AddSingleton(sp =>
        {
            var mongoDatabase = sp.GetRequiredService<IMongoDatabase>();
            return new GridFSBucket(mongoDatabase, new GridFSBucketOptions
            {
                BucketName = options.BucketName,
                ChunkSizeBytes = options.ChunkSizeBytes,
                WriteConcern = WriteConcern.WMajority,
                ReadPreference = ReadPreference.Primary
            });
        });

        services.AddSingleton<IFileService, FileService>();
        return services;
    }

    public static IServiceCollection AddMongoFileStoregeAsScoped<TOptions>(this IServiceCollection services, IConfiguration configuration)
        where TOptions : MongoGridFsFileStorageOptions
    {
        services.AddAndValiadateOptions<TOptions>(configuration);

        services.AddScoped<IFileService<TOptions>>(sp =>
        {
            var options = sp.GetRequiredService<IOptions<TOptions>>();
            var optionsValue = options.Value;
            var client = new MongoClient(configuration.GetConnectionString(optionsValue.ConnectionStringName));
            var db = client.GetDatabase(optionsValue.DatabaseName);
            var bucket = new GridFSBucket(db, new GridFSBucketOptions
            {
                BucketName = optionsValue.BucketName,
                ChunkSizeBytes = optionsValue.ChunkSizeBytes,
                WriteConcern = WriteConcern.WMajority,
                ReadPreference = ReadPreference.Primary
            });

            return new FileService<TOptions>(bucket, options);
        });
        return services;
    }
}
