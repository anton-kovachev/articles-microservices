
namespace Review.Persistence.EntityConfigurations;

public class ReviewerSpecializationEntityConfiguration : IEntityTypeConfiguration<ReviewerSpecialization>
{
    public void Configure(EntityTypeBuilder<ReviewerSpecialization> builder)
    {
        builder.HasKey(e => new { e.ReviewerId, e.JournalId });

        builder.HasOne(e => e.Reviewer)
            .WithMany(e => e.Specializations)
            .HasForeignKey(e => e.ReviewerId)
            .HasPrincipalKey(e => e.Id)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.Journal)
            .WithMany()
            .HasForeignKey(e => e.JournalId)
            .HasPrincipalKey(e => e.Id)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
