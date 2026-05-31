using Blocks.Core.Constraints;
using Blocks.EntityFrameworkCore.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Submission.Domain.Entities;

namespace Submission.Persistence.EntityConfiguration;

public class PersonEntityConfiguration : EntityConfiguration<Person>
{
    public override void Configure(EntityTypeBuilder<Person> builder)
    {
        base.Configure(builder);

        builder.HasIndex(e => e.UserId).IsUnique();
        builder.HasDiscriminator(e => e.TypeDiscriminator)
            .HasValue<Person>(nameof(Person))
            .HasValue<Author>(nameof(Author));

        builder.Property(e => e.FirstName).IsRequired().HasMaxLength(MaxLength.C64);
        builder.Property(e => e.LastName).IsRequired().HasMaxLength(MaxLength.C64);
        builder.Property(e => e.Title).HasMaxLength(MaxLength.C64);
        builder.Property(e => e.Affiliation).HasMaxLength(MaxLength.C512).IsRequired()
            .HasComment("Institution or organization they are associated with when they conduct their research.");
        builder.Property(e => e.UserId).IsRequired(false);

        builder.ComplexProperty(e => e.Email, builder =>
        {
            builder.Property(x => x.Value)
            .HasColumnName(builder.Metadata.PropertyInfo!.Name)
            .HasMaxLength(64);
        });
    }
}
