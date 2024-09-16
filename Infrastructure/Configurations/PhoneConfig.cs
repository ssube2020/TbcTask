using Core.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

public class PhoneConfig : IEntityTypeConfiguration<Phone>
{
    public void Configure(EntityTypeBuilder<Phone> builder)
    {
        builder.ToTable("Phones");

        builder.Property(p => p.PhoneNumber)
            .IsRequired()
            .HasMaxLength(50);
        
        builder.Property(p => p.PhoneNumberType).IsRequired();
    }
}