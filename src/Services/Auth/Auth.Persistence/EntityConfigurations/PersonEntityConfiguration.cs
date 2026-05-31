using Auth.Domain.Persons;
using Auth.Domain.Persons.ValueObjects;
using Blocks.Core.Constraints;
using Blocks.EntityFrameworkCore;
using Blocks.EntityFrameworkCore.EntityConfigurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Auth.Persistence.EntityConfigurations;

public class PersonEntityConfiguration : EntityConfiguration<Person>
{
    public override void Configure(EntityTypeBuilder<Person> builder)
    {
        base.Configure(builder);
        builder.Property(e => e.FirstName).IsRequired().HasMaxLength(MaxLength.C64);
        builder.Property(e => e.LastName).IsRequired().HasMaxLength(MaxLength.C64);
        builder.Property(e => e.Gender).IsRequired().HasEnumConversion();

        builder.OwnsOne(e => e.Email, b =>
        {
            b.Property(x => x.Value)
                .HasMaxLength(MaxLength.C64)
                .HasColumnName(nameof(Person.Email));

            b.Property(x => x.NormalizedEmail)
                .HasMaxLength(MaxLength.C64)
                .HasColumnName(nameof(EmailAddress.NormalizedEmail));
        });

        builder.OwnsOne(e => e.Honorific, b =>
        {
            b.Property(x => x.Value)
                .HasMaxLength(MaxLength.C32)
                .HasColumnNameAsProperty();

            b.WithOwner(); // required to avoid navigation issues
        });
        builder.OwnsOne(e => e.ProfessionalProfile, b =>
        {
            b.Property(x => x.Position)
                .HasMaxLength(MaxLength.C32)
                .HasColumnNameAsProperty();

            b.Property(x => x.CompanyName)
                .HasMaxLength(MaxLength.C32)
                .HasColumnNameAsProperty();

            b.Property(x => x.Affiliation)
                .HasMaxLength(MaxLength.C32)
                .HasColumnNameAsProperty();

            b.WithOwner(); // required to avoid navigation issues
        });

        builder.Property(e => e.PictureUrl).HasMaxLength(MaxLength.C2048);
    }
}
