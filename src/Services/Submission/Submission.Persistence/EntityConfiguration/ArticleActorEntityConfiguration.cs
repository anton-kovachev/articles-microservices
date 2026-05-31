using Articles.Abstractions.Enums;
using Blocks.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Submission.Domain.Entities;

namespace Submission.Persistence.EntityConfiguration;

public class ArticleActorEntityConfiguration : IEntityTypeConfiguration<ArticleActor>
{
    public void Configure(EntityTypeBuilder<ArticleActor> builder)
    {
        builder.HasKey(e => new { e.ArticleId, e.PersonId, e.Role });
        builder.HasDiscriminator(e => e.TypeDiscriminator)
            .HasValue<ArticleActor>(nameof(ArticleActor))
            .HasValue<ArticleAuthor>(nameof(ArticleAuthor));

        builder.Property(e => e.Role).HasEnumConversion().HasDefaultValue(UserRoleType.AUT);

        builder.HasOne(e => e.Article)
            .WithMany(e => e.Actors)
            .HasForeignKey(e => e.ArticleId)
            .OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(e => e.Person)
            .WithMany(e => e.Actors)
            .HasForeignKey(e => e.PersonId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
