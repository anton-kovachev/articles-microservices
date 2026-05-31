using Microsoft.AspNetCore.Authorization;
using FastEndpoints;
using Blocks.Redis;
using Mapster;
using Articles.Abstractions;
using Journals.Domain.Journal;
using Journals.Domain.Journal.Events;
using Auth.Grpc;
using Grpc.Core;
using Articles.Security;

namespace Journals.Api.Features.Journals.Create;

[Authorize(Roles = Role.EOF)]
[HttpPost("journals")]
[Tags("Journals")]
public class CreateJournalEndpoint(Repository<Journal> _journalRepository, Repository<Editor> _editorRepository, IPersonService _persionService) : Endpoint<CreateJournalCommand, IdResponse>
{
    public override async Task HandleAsync(CreateJournalCommand command, CancellationToken cancellationToken)
    {
        if (_journalRepository.Collection.Any(j => j.Abbbreviation == command.Abbreviation || j.Name == command.Name))
            throw new BadHttpRequestException("A journal with the same name or abbreviation already exists.");
        
        if (!_editorRepository.Collection.Any(e => e.Id == command.ChiefEditorId))
            await CreateEditor(command.ChiefEditorId, cancellationToken);

        var journal = command.Adapt<Journal>();
        await _journalRepository.AddAsync(journal);
        await _journalRepository.SaveAllAsync();

        await PublishAsync(new JournalCreated(journal));
        await Send.OkAsync(new IdResponse(journal.Id));
    }

    private async Task CreateEditor(int userId, CancellationToken cancellationToken)
    {
        var response = await _persionService.GetPersonByUserIdAsync(
            new GetPersonByIdRequest { UserId = userId },
            new CallOptions(cancellationToken: cancellationToken));

        var editor = new Editor
        {
            Id = userId,
            PersonId = response.PersonInfo.Id,
            Affiliation = response.PersonInfo.ProfessionalProfile!.Affiliation,
            FullName = response.PersonInfo.FirstName + " " + response.PersonInfo.LastName
        };

        await _editorRepository.AddAsync(editor);
    }
}
