namespace FileStorage.Contract;

public record UploadResponse(string FilePath, string FileName, long FileSize, string FileId);
