using Review.Domain.Reviewers;

namespace Review.Persistence.EntityConfigurations;

public class ReviewerEntityConfiguration : IEntityTypeConfiguration<Reviewer>
{
    public void Configure(EntityTypeBuilder<Reviewer> builder)
    {
        builder.HasBaseType<Person>();
        builder.HasMany(e => e.Specializations).WithOne(e => e.Reviewer);
    }
}
