
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Blocks.EntityFrameworkCore;
using Submission.Domain.Entities;

namespace Submission.Persistence.EntityConfiguration;

public class ArticleAuthorEntityConfiguration : IEntityTypeConfiguration<ArticleAuthor>
{
    public void Configure(EntityTypeBuilder<ArticleAuthor> builder)
    {
        builder.Property(e => e.ContributionAreas).HasJsonCollectionConversion().IsRequired();
    }
}
