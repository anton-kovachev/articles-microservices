using Review.Domain.Assets;

namespace Review.Persistence.EntityConfigurations;

public class AssetTypeDefinitionConfiguration : EnumEntityConfiguration<AssetTypeDefinition, AssetType>
{
    public override void Configure(EntityTypeBuilder<AssetTypeDefinition> builder)
    {
        base.Configure(builder);    
        builder.Property(e => e.MaxAssetCount).HasDefaultValue(1);
        builder.Property(e => e.DefaultFileExtension).IsRequired().HasMaxLength(MaxLength.C8).HasDefaultValue("pdf");

        builder.ComplexProperty(e => e.AllowedFileExtensions, builder =>
        {
            var conversion = BuilderExtensions.BuildJsonReadOnlyListConverter<string>();
            builder.Property(fe => fe.Extensions)
            .IsRequired()
            .HasColumnName(builder.Metadata.PropertyInfo!.Name)
            .HasConversion(conversion);
        });
    }
}
