using Core.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;

public class ConnectedPersonConfig : IEntityTypeConfiguration<ConnectedPerson>
{
    public void Configure(EntityTypeBuilder<ConnectedPerson> builder)
    {
        builder.ToTable("ConnectedPersons");
        
        builder.Property(p => p.ConnectedPersonId).IsRequired();
        builder.Property(p => p.ConnectionType).IsRequired();
    }
}
