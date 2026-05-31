namespace Review.Persistence.EntityConfigurations;

public class FileEntityConfiguration
{
    public void Configure(ComplexPropertyBuilder<Domain.Assets.ValueObjects.File> builder)
    {
        builder.Property(e => e.FileServerId).HasMaxLength(MaxLength.C64);
        builder.Property(e => e.OriginalName).HasMaxLength(MaxLength.C256).HasComment("Original full file name, with extension");
        builder.Property(e => e.Size).HasComment("Size of the file in kilobytes");

        builder.ComplexProperty(o => o.Extension, complexBuilder =>
        {
            complexBuilder.Property(e => e.Value)
                .HasMaxLength(MaxLength.C16)
                .HasColumnName($"{complexBuilder.Metadata.ClrType.IsNestedAssembly}_{complexBuilder.Metadata.PropertyInfo!.Name}");

            builder.ComplexProperty(o => o.Name, complexBuilder =>
            {
                complexBuilder.Property(e => e.Value)
                    .HasMaxLength(MaxLength.C64)
                    .HasColumnName($"{complexBuilder.Metadata.ClrType.IsNestedAssembly}_{complexBuilder.Metadata.PropertyInfo!.Name}")
                    .HasComment("Final name of the file after renaming");
            });
        });
    }
}
