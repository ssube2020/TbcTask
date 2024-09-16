using Core.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

internal class UserConfig : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");
        
        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(x => x.Surname)
            .IsRequired()
            .HasMaxLength(50);
        
        builder.Property(x => x.Gender).IsRequired();
        
        builder.Property(x => x.PersonalNumber)
            .IsRequired()
            .HasMaxLength(50);
        
        builder.Property(x => x.BirthDate).IsRequired();
        
        builder.Property(x => x.City).IsRequired();
        
        builder.HasMany(u => u.PhoneNumbers)
            .WithOne(p => p.User)
            .HasForeignKey(p => p.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(u => u.Connections)
            .WithOne(c => c.User)
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.Restrict); 
    }
}
