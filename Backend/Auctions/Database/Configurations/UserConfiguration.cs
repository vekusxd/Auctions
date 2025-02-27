using Auctions.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Auctions.Database.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");
        
        builder
            .HasKey(u => u.Id);
        
        builder
            .Property(u => u.FirstName)
            .HasMaxLength(50)
            .IsRequired();

        builder
            .Property(u => u.LastName)
            .HasMaxLength(50)
            .IsRequired();
    }
}