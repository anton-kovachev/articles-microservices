using Blocks.Core.Constraints;

namespace ArticleHub.Persistence.EntityConfigurations;

public class ArticleEntityConfiguration : EntityConfiguration<Article>
{
    public override bool HasGeneratedId => false;

    public override void Configure(EntityTypeBuilder<Article> builder)
    {
        base.Configure(builder);
        builder.HasIndex(e => e.Title);

        builder.Property(e => e.Title).IsRequired().HasMaxLength(MaxLength.C256);
        builder.Property(e => e.Doi).HasMaxLength(MaxLength.C64);
        builder.Property(e => e.Stage).HasEnumConversion().IsRequired();

        builder.Property(e => e.SubmittedOn).HasColumnType("timestamp without time zone").IsRequired();
        builder.Property(e => e.AcceptedOn).HasColumnType("timestamp without time zone");
        builder.Property(e => e.PublishedOn).HasColumnType("timestamp without time zone");

        builder.HasOne(e => e.SubmittedBy).WithMany()
            .HasForeignKey(e => e.SubmittedById)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);


        builder.HasOne(e => e.Journal).WithMany(e => e.Articles)
            .HasForeignKey(e => e.JournalId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
    }
}
