namespace Review.Persistence.EntityConfigurations;

public class ArticleActorEntityConfiguration : IEntityTypeConfiguration<ArticleActor>
{
    public void Configure(EntityTypeBuilder<ArticleActor> builder)
    {
        builder.HasKey(e => new { e.ArticleId, e.PersonId, e.Role });

        builder.Property(e => e.Role).HasEnumConversion().HasDefaultValue(UserRoleType.AUT);
        builder.HasDiscriminator(e => e.TypeDiscriminator)
            .HasValue<ArticleActor>(nameof(ArticleActor))
            .HasValue<ArticleAuthor>(nameof(ArticleAuthor));

        builder.HasOne(e => e.Article)
            .WithMany(e => e.Actors)
            .HasForeignKey(e => e.ArticleId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(e => e.Person)
            .WithMany()
            .HasForeignKey(e => e.PersonId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
