using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blocks.EntityFrameworkCore.EntityConfigurations;

public abstract class EntityConfiguration<T> : EntityConfiguration<T, int>
    where T : class, IEntity<int>
{
    public virtual bool HasGeneratedId => true;

    public override void Configure(EntityTypeBuilder<T> builder)
    {
        base.Configure(builder);
        if (HasGeneratedId)
            builder.Property(e => e.Id).IsRequired().ValueGeneratedOnAdd().HasColumnOrder(0);
        else
            builder.Property(e => e.Id).IsRequired().ValueGeneratedNever().HasColumnOrder(0);
    }
}

public abstract class EntityConfiguration<T, TKey> : IEntityTypeConfiguration<T>
    where T : class, IEntity<TKey>
    where TKey : struct
{
    public virtual void Configure(EntityTypeBuilder<T> builder)
    {
        builder.HasKey(e => e.Id);
    }

    public virtual string EntityName => typeof(T).Name;
}
