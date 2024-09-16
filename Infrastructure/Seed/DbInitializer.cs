using Core.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Infrastructure.DataContext;

namespace Infrastructure.Seed;

public static class DbInitializer
{
    public static void Initialize(IServiceProvider serviceProvider)
    {
        using IServiceScope serviceScope = serviceProvider.CreateScope();

        ApplicationDbContext context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();

        if (context.Database.GetPendingMigrations().Any())
        {
            context.Database.Migrate();
        }
    }

    public static void Seed(ModelBuilder builder)
    {
        builder
            .Entity<User>()
            .HasData(new User
            {
                Id = -1,
                Name = "DefaultName",
                Surname = "DefaultSurname",
                Gender = GenderEnum.Male,
                PersonalNumber = "00000000000",
                BirthDate = DateTime.Now.Date,
                City = "Tbilisi",
                PhoneNumbers = new() {},
                Connections = new() {}
            });
    }
}