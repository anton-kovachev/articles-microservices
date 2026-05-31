using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Blocks.Core.Constraints;
using Blocks.EntityFrameworkCore;
using Submission.Domain.Entities;

namespace Submission.Persistence.EntityConfiguration;

internal class AssetTypeDefinitionEntityConfiguration : IEntityTypeConfiguration<AssetTypeDefinition>
{
    public void Configure(EntityTypeBuilder<AssetTypeDefinition> builder)
    {
        builder.HasKey(e => e.Id);
        builder.HasIndex(e => e.Name).IsUnique();

        builder.Property(e => e.Name).HasMaxLength(MaxLength.C64).IsRequired().HasColumnOrder(1);
        builder.Property(e => e.MaxFileSizeInMB).HasDefaultValue(5); // 5 MB
        builder.Property(e => e.DefaultFileExtension).HasMaxLength(MaxLength.C8).HasDefaultValue("pdf").IsRequired();

        builder.ComplexProperty(e => e.AllowedFileExtensions, builder =>
        {
            var converter = BuilderExtensions.BuildJsonReadOnlyListConverter<string>();
            builder.Property(x => x.Extensions)
                .HasConversion(converter)
                .HasColumnName(builder.Metadata.PropertyInfo!.Name)
                .IsRequired();
        });
    }
}
