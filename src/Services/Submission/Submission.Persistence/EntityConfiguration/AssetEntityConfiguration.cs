
using Blocks.EntityFrameworkCore;
using Blocks.EntityFrameworkCore.EntityConfigurations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Submission.Domain.Entities;
using Blocks.Core.Constraints;

namespace Submission.Persistence.EntityConfiguration;

internal class AssetEntityConfiguration : EntityConfiguration<Asset>
{
    public override void Configure(EntityTypeBuilder<Asset> builder)
    {
        base.Configure(builder);
        builder.Property(e => e.Type).HasEnumConversion();
        builder.ComplexProperty(e => e.Name, builder => 
        { 
            builder.Property(n => n.Value)
                .HasColumnName(builder.Metadata.PropertyInfo!.Name)
                .HasMaxLength(MaxLength.C64)
                .IsRequired();
        });

        builder.ComplexProperty(e => e.File, builder =>
        {
            new FileEntityConfiguration().Configure(builder);
        });
    }
}
