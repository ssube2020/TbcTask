using System.Reflection;
using Core.Models.Entities;
using Infrastructure.Seed;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DataContext;
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    
    public DbSet<User?> Users { get; set; }
    public DbSet<Phone> Phones { get; set; }
    public DbSet<ConnectedPerson> ConnectedPersons { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfigurationsFromAssembly(
            Assembly.GetExecutingAssembly(),
            t => t.GetInterfaces().Any(i =>
                i.IsGenericType &&
                i.GetGenericTypeDefinition() == typeof(IEntityTypeConfiguration<>)));

        DbInitializer.Seed(builder);
    }
}