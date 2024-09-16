using Core.Models.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.DataContext;

// public class ApplicationDbContextFactory(IConfiguration configuration) : IDesignTimeDbContextFactory<ApplicationDbContext>
// {
//     public ApplicationDbContext CreateDbContext(string[] args)
//     {
//         var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
//         optionsBuilder.UseSqlServer(configuration.GetConnectionString(ConfigurationConstants.SqlConnection));
//         //optionsBuilder.UseSqlServer("Server=localhost,1400;Database=UsersDb;User Id=sa;Password=Test1234;TrustServerCertificate=True");
//         
//         return new ApplicationDbContext(optionsBuilder.Options);
//     }
// }
