using Domain.Entities;
using FileStorage.Contract;

namespace Submission.Domain.Entities;

public partial class Asset 
{
    private Asset() { }
    internal static Asset Create(Article article, AssetTypeDefinition type)
    {
        return new()
        {
            ArticleId = article.Id,
            Article = article,
            Name = AssetName.FromAssetType(type),
            Type = type.Id
        };
    }

    public string GenerateStorageFilePath(string fileName)
        => $"Articles/{ArticleId}/{Name}/{fileName}";

    public File CreateFile(FileMetadata fileMetadata, AssetTypeDefinition assetType)
    {
        File = File.CreateFile(fileMetadata, this, assetType);
        return File;
    }
}
