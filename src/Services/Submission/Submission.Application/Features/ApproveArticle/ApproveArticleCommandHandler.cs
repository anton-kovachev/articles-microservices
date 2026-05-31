using Auth.Grpc;
using Blocks.Core;
using Blocks.Exceptions;
using Blocks.EntityFrameworkCore;
using Grpc.Core;
using Journals.Grpc;

namespace Submission.Application.Features.ApproveArticle;

public class ApproveArticleCommandHandler(
    ArticleRepository _articleRepository, 
    PersonRepository _personRepository, 
    IPersonService _personClient,
    IJournalService _journalService) : 
    IRequestHandler<ApproveArticleCommand, IdResponse>
{
    public async Task<IdResponse> Handle(ApproveArticleCommand command, CancellationToken cancellationToken)
    {
        var article = await _articleRepository.FindByIdOrThrowAsync(command.ArticleId);
        bool isAssigned = await IsEditorAssignedToJournal(command, article);

        if (!isAssigned)
            throw new BadRequestException($"Editor is not assigned to the article's journal {article.JournalId}");

        Person editor = await GetOrCreatePersonByUserId(command.CreatedById, cancellationToken);
        article.Approve(editor);
        await _articleRepository.SaveChangesAsync(cancellationToken);
        return new IdResponse(article.Id);
    }

    private async Task<bool> IsEditorAssignedToJournal(ApproveArticleCommand command, Article article)
    {
        var response = await _journalService.IsEditorAssignedToJournalAsync(new IsEditorAssignedToJournalRequest
        {
            JournalId = article.JournalId,
            UserId = command.CreatedById
        });

        return response.IsAssigned;
    }

    private async Task<Person> GetOrCreatePersonByUserId(int userId, CancellationToken cancellationToken)
    {
        var person = Guard.NotFound(await _personRepository.GetByUserIdAsync(userId));
        if (person is null)
        {
            var response = await _personClient.GetPersonByUserIdAsync(new GetPersonByIdRequest { UserId = userId }, new CallOptions(cancellationToken: cancellationToken));
            person = Person.Create(response.PersonInfo);

            await _personRepository.AddAsync(person, cancellationToken);
            await _personRepository.SaveChangesAsync(cancellationToken);
        }

        return person;
    }
}
