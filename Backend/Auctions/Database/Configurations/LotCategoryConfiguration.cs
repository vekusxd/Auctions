using Auctions.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Auctions.Database.Configurations;

public class LotCategoryConfiguration : IEntityTypeConfiguration<LotCategory>
{
    public void Configure(EntityTypeBuilder<LotCategory> builder)
    {
        builder.HasKey(c => c.Id);

        builder.ToTable("LotCategories");

        builder
            .Property(c => c.Title)
            .HasMaxLength(100)
            .IsRequired();

        builder
            .Property(c => c.Description)
            .HasMaxLength(-1);

        builder
            .HasMany(c => c.Lots)
            .WithOne(l => l.LotCategory)
            .HasForeignKey(l => l.LotCategoryId);
    }
}