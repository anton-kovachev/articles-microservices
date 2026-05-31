using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Submission.Domain.Entities;

namespace Submission.Persistence.EntityConfiguration;

public class AuthorEntityConfiguration : IEntityTypeConfiguration<Author>
{
    public void Configure(EntityTypeBuilder<Author> builder)
    {
        builder.Property(e => e.Discipline).HasMaxLength(64)
            .HasComment("The author's main filed of study research (e.g., Biology, Computer Science).");
        builder.Property(e => e.Degree).HasMaxLength(64)
            .HasComment("The author's highest academic qualification (e.g. PhD in Mathematics, MSc in Chemistry).");

    }
}
