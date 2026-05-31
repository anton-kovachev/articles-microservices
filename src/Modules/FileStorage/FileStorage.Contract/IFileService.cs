using Microsoft.AspNetCore.Http;

namespace FileStorage.Contract;

public interface IFileStorageOptions;

public interface IFileService<TFileStorageOptions> : IFileService
    where TFileStorageOptions : IFileStorageOptions;

public interface IFileService
{
    Task<FileMetadata> UploadFileAsync(string filePath, IFormFile file, bool overwrite = false, Dictionary<string, string>? tags = null, CancellationToken cancellationToken = default);
    Task<FileMetadata> UploadFileAsync(FileUploadRequest file, Stream stream, bool overwrite = false, Dictionary<string, string>? tags = null, CancellationToken cancellationToken = default);
    Task<(Stream FileStream, FileMetadata FileMetadata)> DownloadFileAsync(string fileId);
    Task<bool> TryDeleteFileAsync(string fileId, CancellationToken cancellationToken = default);
}


public record FileUploadRequest(string StoragePath, string FileName, string ContentType, long FileSize = default);
public record FileMetadata(string StoragePath, string FileName, string ContentType, long FileSize, string FileId);
