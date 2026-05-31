
using Blocks.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Blocks.EntityFrameworkCore;

public static class MediatorExtension
{
    public static async Task<int> DispatchDomainEventAsync(this IMediator mediator, DbContext dbContext, CancellationToken cancellationToken)
    {
        var domainAggregates = dbContext.ChangeTracker.Entries<IAggregateRoot>();
        var domainEvents = domainAggregates.SelectMany(d => d.Entity.DomainEvents).ToList();

        domainAggregates.ToList().ForEach(d => d.Entity.ClearDomainEvents());

        foreach(var domainEvent in domainEvents)
            await mediator.Publish(domainEvent, cancellationToken);

        return domainEvents.Count;

    }
}
