using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using FileStorage.Contract;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;

namespace FileStorage.MongoGridFS;

public class FileService(GridFSBucket bucket, IOptions<MongoGridFsFileStorageOptions> options)
    : FileService<MongoGridFsFileStorageOptions>(bucket, options)
{
}

public class FileService<TFileStorageOptions> : IFileService<TFileStorageOptions>
    where TFileStorageOptions : MongoGridFsFileStorageOptions
{
    private readonly GridFSBucket _bucket;
    private readonly TFileStorageOptions _options;

    private const string FilePathMetadataKey = "filePath";
    private const string ContentTypeMetadataKey = "contentType";
    private const string DefaultContentType = "application/octet-stream";

    public FileService(GridFSBucket bucket, IOptions<TFileStorageOptions> options)
        => (_bucket, _options) = (bucket, options.Value);

    public async Task<FileMetadata> UploadFileAsync(string storagePath, IFormFile file, bool overwrite = false, Dictionary<string, string>? tags = null, CancellationToken cancellationToken = default)
    {
        var request = new FileUploadRequest(storagePath, file.FileName, file.ContentType, file.Length);
        using var stream = file.OpenReadStream();

        return await UploadInternalAsync(request, stream, overwrite, tags, cancellationToken);
    }

    public async Task<FileMetadata> UploadFileAsync(FileUploadRequest file, Stream stream, bool overwrite = false, Dictionary<string, string>? tags = null, CancellationToken cancellationToken = default)
    {
        return await UploadInternalAsync(file, stream, overwrite, tags, cancellationToken);
    }

    private async Task<FileMetadata> UploadInternalAsync(FileUploadRequest request, Stream stream, bool overwrite = false, Dictionary<string, string>? tags = null, CancellationToken cancellationToken = default)
    {
        if (request.FileSize > _options.FileSizeLimitInBytes)
            throw new InvalidOperationException($"File exceeds maximum allowed size of {_options.FileSizeLimitInBytes} bytes");

        if (overwrite)
            await TryDeleteFileAsync(request.StoragePath, cancellationToken);

        var metadata = new BsonDocument(tags ?? new Dictionary<string, string>
        {
            { nameof(FileMetadata.StoragePath), request.StoragePath },
            { nameof(FileMetadata.FileName), request.ContentType },
            { nameof(FileMetadata.ContentType), request.ContentType ?? DefaultContentType }
        });

        var uploadOptiosn = new GridFSUploadOptions
        {
            Metadata = metadata,
            ChunkSizeBytes = _options.ChunkSizeBytes
        };

        ObjectId fileId;
        fileId = await _bucket.UploadFromStreamAsync(request.FileName, stream, uploadOptiosn, cancellationToken);

        return new FileMetadata(
            StoragePath: request.StoragePath,
            FileName: request.FileName,
            ContentType: request.ContentType ?? DefaultContentType,
            FileSize: request.FileSize,
            FileId: fileId.ToString());
    }

    public async Task<(Stream FileStream, FileMetadata FileMetadata)> DownloadFileAsync(string fileId)
    {
        if (!ObjectId.TryParse(fileId, out var objectId))
            throw new FileNotFoundException($"Invalid fileId supplied {fileId}");

        var fileInfo = await _bucket.Find(Builders<GridFSFileInfo>.Filter.Eq(f => f.Id, objectId)).FirstOrDefaultAsync();

        if (fileInfo == null)
            throw new FileNotFoundException($"No file found with id of {fileId}");

        using var stream = await _bucket.OpenDownloadStreamAsync(objectId);
        var metadata = fileInfo.Metadata;

        var fileMatadata = new FileMetadata(
            StoragePath: metadata.GetValue(nameof(FileMetadata.StoragePath), string.Empty).AsString,
            FileName: metadata.GetValue(nameof(FileMetadata.FileName), string.Empty).AsString,
            ContentType: metadata.GetValue(nameof(FileMetadata.ContentType), DefaultContentType).AsString,
            FileSize: fileInfo.Length,
            FileId: fileId);

        return (FileStream: stream, FileMetadata: fileMatadata);
    }

    public async Task<bool> TryDeleteFileAsync(string fileId, CancellationToken cancellationToken = default)
    {
        if (!ObjectId.TryParse(fileId, out var objectId))
            return false;

        try
        {
            await _bucket.DeleteAsync(objectId, cancellationToken);
        }
        catch (GridFSFileNotFoundException)
        {
            return false;
        }

        return true;
    }
}
