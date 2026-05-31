namespace ArticleHub.Persistence.EntityConfigurations;

internal class ArticleActorEntityConfiguration : IEntityTypeConfiguration<ArticleActor>
{
    public void Configure(EntityTypeBuilder<ArticleActor> builder)
    {
        builder.HasKey(e => new { e.ArticleId, e.PersonId, e.Role });
        builder.Property(e => e.Role).HasEnumConversion().IsRequired().HasDefaultValue(UserRoleType.AUT);

        builder.HasOne(e => e.Article)
            .WithMany(e => e.Actors)
            .HasForeignKey(e => e.ArticleId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(e => e.Person)
            .WithMany()
            .HasForeignKey(e => e.PersonId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
    }
}
