using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Submission.Domain.Entities;
using Blocks.EntityFrameworkCore;
using Blocks.EntityFrameworkCore.EntityConfigurations;

namespace Submission.Persistence.EntityConfiguration;

internal class ArticleEntityConfiguration : EntityConfiguration<Article>
{
    public override void Configure(EntityTypeBuilder<Article> builder)
    {
        base.Configure(builder);

        builder.Property(e => e.Title).HasMaxLength(256).IsRequired();
        builder.Property(e => e.Scope).HasMaxLength(2048).IsRequired();
        builder.Property(e => e.Stage).HasEnumConversion();
        builder.Property(e => e.Type).HasEnumConversion();

        builder.HasOne(e => e.Journal)
            .WithMany()
            .HasForeignKey(e => e.JournalId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(e => e.Assets)
            .WithOne(e => e.Article)
            .HasForeignKey(e => e.ArticleId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }
}
