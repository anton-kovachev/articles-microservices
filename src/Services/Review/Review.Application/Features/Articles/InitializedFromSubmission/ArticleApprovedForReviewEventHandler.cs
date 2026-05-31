
using Articles.IntregationEvents.Contracts;
using Articles.IntregationEvents.Contracts.Articles.Dtos;
using Blocks.Domain;
using Blocks.Exceptions;
using FileStorage.Contract;
using Mapster;
using MassTransit;
using Review.Application.FileStorage;
using Review.Domain.Articles;
using Review.Domain.Assets;
using Review.Domain.Shared;

namespace Review.Application.Features.Articles.InitializedFromSubmission;

public class ArticleApprovedForReviewEventHandler(
    ReviewDbContext reviewDbContext,
    ArticleRepository _articleRepository,
    Repository<Journal> _journalRepository,
    Repository<Person> _personRepository,
    AssetTypeDefinitionRepository _assetTypeDefinitionRepository,
    IFileService _fileService,
    FileServiceFactory _fileServiceFactory) : IConsumer<ArticleApprovedForReviewEvent>
{
    public async Task Consume(ConsumeContext<ArticleApprovedForReviewEvent> context)
    {
        var articleDto = context.Message.Article;
        if (_articleRepository.Entity.Any(e => e.Id == articleDto.Id))
            throw new BadRequestException($"Article with id {articleDto.Id} already exists in the review database.");

        var journal = await GetOrCreateJournalAsync(articleDto);
        var actors = await CreateActors(articleDto);

        var assets = await CreateAssetsAsync(articleDto);
        var article = Article.AcceptSubmitted(articleDto, actors, assets);

        journal.AddArticle(article);
        await reviewDbContext.SaveChangesAsync(context.CancellationToken);
    }

    private async Task<IEnumerable<Asset>> CreateAssetsAsync(ArticleDto articleDto)
    {
        var assets = new List<Asset>();
        foreach (var assetDto in articleDto.Assets)
        {
            var assetTypeDefinition = _assetTypeDefinitionRepository!.GetById(assetDto.Type);
            var asset = Asset.CreateFromSubmission(assetDto, assetTypeDefinition, articleDto.Id);

            //TODO: Get the file from submission file storage and store it in the review file storage
            var submissionFileService = _fileServiceFactory(FileStorageType.Submission);
            var (fileStream, fileMetadata) =  await submissionFileService.DownloadFileAsync(assetDto.File.FileServerId);

            var fileUploadRequest = new FileUploadRequest(fileMetadata.StoragePath, fileMetadata.FileName, fileMetadata.ContentType, fileMetadata.FileSize);
            fileMetadata = await _fileService.UploadFileAsync(fileUploadRequest, fileStream);

            asset.CreateFile(fileMetadata, assetTypeDefinition);
            assets.Add(asset);
        }

        return assets;
    }

    private async Task<IEnumerable<ArticleActor>> CreateActors(ArticleDto articleDto)
    {
        var actors = new List<ArticleActor>();
        foreach (var actorDto in articleDto.Actors)
        {
            var person = await _personRepository.GetByIdAsync(actorDto.Person.Id);
            ArticleActor actor = default!;

            if (actorDto.Role == UserRoleType.AUT || actorDto.Role == UserRoleType.CORAUT)
            {
                if (person is null)
                    person = actorDto.Person.Adapt<Person>();

                actor = new ArticleAuthor(actorDto.ContributionAreas)
                {
                    PersonId = person.Id,
                    Person = person,
                    Role = actorDto.Role,
                };
            }
            else if (actorDto.Role == UserRoleType.REVED)
            {
                if (person is null)
                    person = actorDto.Person.Adapt<Editor>();

                actor = new ArticleActor
                {
                    PersonId = person.Id,
                    Person = person,
                    Role = actorDto.Role,
                };
            }
            else
            {
                throw new DomainException($"Unknown role for {actorDto.Person.Email}");
            }

            actors.Add(actor);
        }

        return actors;
    }

    private async Task<Journal> GetOrCreateJournalAsync(ArticleDto articleDto)
    {
        var journal = await _journalRepository.FindByIdAsync(articleDto.Journal.Id);
        if (journal is null)
        {
            journal = articleDto.Journal.Adapt<Journal>();
            await _journalRepository.AddAsync(journal);
        }

        return journal;
    }
}
