using Auctions.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Auctions.Database.Configurations;

public class LotConfiguration : IEntityTypeConfiguration<Lot>
{
    public void Configure(EntityTypeBuilder<Lot> builder)
    {
        builder.HasKey(l => l.Id);

        builder.ToTable("Lots");
        
        builder
            .Property(l => l.Title)
            .HasMaxLength(100)
            .IsRequired();
        
        builder
            .Property(l => l.Description)
            .HasMaxLength(-1);

        builder
            .HasOne(l => l.LotCategory)
            .WithMany(lc => lc.Lots)
            .HasForeignKey(l => l.LotCategoryId);

        builder
            .Property(l => l.ImgUrl)
            .HasMaxLength(255);

        builder
            .HasOne(l => l.Seller)
            .WithMany(u => u.Lots)
            .HasForeignKey(l => l.SellerId);

        builder
            .HasMany(l => l.Bids)
            .WithOne(b => b.Lot)
            .HasForeignKey(b => b.LotId);
    }
}