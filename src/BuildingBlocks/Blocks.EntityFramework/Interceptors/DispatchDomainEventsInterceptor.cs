using MediatR;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Blocks.EntityFrameworkCore.Interceptors;

public class DispatchDomainEventsInterceptor(IMediator _mediator) : SaveChangesInterceptor
{
    public override Task SaveChangesFailedAsync(DbContextErrorEventData eventData, CancellationToken cancellationToken = default)
    {
        return base.SaveChangesFailedAsync(eventData, cancellationToken);
    }

    public override async ValueTask<int> SavedChangesAsync(SaveChangesCompletedEventData eventData, int result, CancellationToken cancellationToken = default)
    {
        result = await base.SavedChangesAsync(eventData, result, cancellationToken);
        if (eventData.Context is not null)
            return await _mediator.DispatchDomainEventAsync(eventData.Context, cancellationToken);

        return result;
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}
