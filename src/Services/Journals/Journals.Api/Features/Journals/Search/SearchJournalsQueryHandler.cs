using Articles.Security;
using Blocks.Core.Extensions;
using Blocks.Redis;
using FastEndpoints;
using Journals.Api.Features._Shared;
using Journals.Domain.Journal;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Redis.OM;

namespace Journals.Api.Features.Journals.Search;

[Authorize(Roles = $"{Role.EOF},{Role.REVED}")]
[HttpGet("journals")]
[Tags("Journals")]
public class SearchJournalsQueryHandler(Repository<Journal> _journalRepository, Repository<Editor> _editorRepository) : Endpoint<SearchJournalsQuery, SearchJournalsResponse>
{
    public async override Task HandleAsync(SearchJournalsQuery query, CancellationToken cancellationToken)
    {
        var collection = _journalRepository.Collection;
        if (!query.Search.IsNullOrEmpty())
        {
            var search = query.Search.Trim().ToLowerInvariant();
            var queryString = 
                $"(@Abbreviation:{{{search}}}) | " + 
                $"(@Name:*{search}*) | @Description:*{search}*)";

            collection = _journalRepository.Collection.Raw(queryString);
            var totalCount = collection.Count();

            var items = collection
                .Skip((query.Page - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToList();

            var response = new SearchJournalsResponse(
                query.Page, 
                query.PageSize, 
                TotalCount: totalCount, 
                Items: items.Select(i => 
                {
                    var dto = i.Adapt<JournalDto>();
                    dto.Editor = (_editorRepository.GetById(i.ChiefEditorId)).Adapt<EditorDto>();
                    
                    return dto;
                }));

            await Send.OkAsync(response, cancellationToken);
        }
    }
}
