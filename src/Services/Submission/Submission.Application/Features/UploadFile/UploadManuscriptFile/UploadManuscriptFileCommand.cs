
using Blocks.Core.FluentValidation;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.Numerics;

namespace Submission.Application.Features.UploadFile.UploadManuscriptFile;

public record UploadManuscriptFileCommand : ArticleCommand
{
    //<summary>
    //The asset type of the file
    //</summary>
    [Required]
    public AssetType AssetType { get; init; }
    //<summary>
    //The file tyo be uploaded 
    //</summary>
    [Required]
    public IFormFile File { get; init; } = null!;
    public override ArticleActionType ActionType => ArticleActionType.Upload;
}

public class UploadedManusciptCommandValidator : ArticleCommandValidator<UploadManuscriptFileCommand>
{
    public UploadedManusciptCommandValidator() 
    {
        RuleFor(x => x.File).NotNullWithMessage(nameof(UploadManuscriptFileCommand.File));
        //TODO: validate file size and file extension
        RuleFor(x => x.AssetType).Must(IsAllowedAssetType).WithMessage(x => $"{x.AssetType} not allowed");
    }

    private bool IsAllowedAssetType(AssetType assertType) => AllowedAssetTypes.Contains(assertType);

    public IReadOnlyCollection<AssetType> AllowedAssetTypes = new HashSet<AssetType> { AssetType.Manuscript };
}
