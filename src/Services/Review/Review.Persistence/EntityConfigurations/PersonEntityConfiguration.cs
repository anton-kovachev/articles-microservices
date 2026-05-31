using Review.Domain.Reviewers;

namespace Review.Persistence.EntityConfigurations;

public class PersonEntityConfiguration : EntityConfiguration<Person>
{
    public override bool HasGeneratedId => false;

    public override void Configure(EntityTypeBuilder<Person> builder)
    {
        base.Configure(builder);

        builder.HasIndex(e => e.UserId).IsUnique();
        // builder.HasIndex(e => new { e.TypeDiscriminator, e.Email });
        // using RawSql to create a unique index on Email and TypeDiscriminator because EF Core does not support unique indexes on properties that are value objects.
        builder.HasAnnotation(
            "RawSql:Index", 
            "CREATE UNIQUE INDEX IX_Person_Email_TypeDiscriminator ON Person (Email, TypeDiscriminator)");

        builder.HasDiscriminator(e => e.TypeDiscriminator)
            .HasValue<Person>(nameof(Person))
            .HasValue<Author>(nameof(Author))
            .HasValue<Reviewer>(nameof(Reviewer))
            .HasValue<Editor>(nameof(Editor));

        builder.Property(e => e.UserId).IsRequired(false);
        builder.Property(e => e.FirstName).IsRequired().HasMaxLength(MaxLength.C64);
        builder.Property(e => e.LastName).IsRequired().HasMaxLength(MaxLength.C64);
        builder.Property(e => e.Honorific).HasMaxLength(MaxLength.C64);
        builder.Property(e => e.Affiliation).HasMaxLength(MaxLength.C512).IsRequired()
            .HasComment("Institution or organization they are associated with when they conduct their research.");

        builder.ComplexProperty(e => e.Email, builder =>
        {
            builder.Property(x => x.Value)
            .HasColumnName(builder.Metadata.PropertyInfo!.Name)
            .HasMaxLength(MaxLength.C64);
        });
    }
}
