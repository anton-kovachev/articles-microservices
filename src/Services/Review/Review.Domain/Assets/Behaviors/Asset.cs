using Articles.IntregationEvents.Contracts.Articles.Dtos;
using FileStorage.Contract;
using Review.Domain.Assets.Enums;
using Review.Domain.Assets.ValueObjects;
using File = Review.Domain.Assets.ValueObjects.File;

namespace Review.Domain.Assets;

public partial class Asset
{
    public static Asset CreateFromSubmission(AssetDto assetDto, AssetTypeDefinition type, int articleId)
    {
        var asset = new Asset()
        {
            ArticleId = articleId,
            Name = AssetName.FromAssetType(type),
            Type = type.Id,
            State = AssetState.Uploaded,
        };

        return asset;
    }

    public File CreateFile(FileMetadata fileMetadata, AssetTypeDefinition assetTypeDefinition)
    {
        File = ValueObjects.File.CreateFile(fileMetadata, this, assetTypeDefinition);
        State = AssetState.Uploaded;

        return File;
    }
}
