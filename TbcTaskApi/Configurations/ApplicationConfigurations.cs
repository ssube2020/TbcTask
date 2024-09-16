using System.Globalization;
using Core.Contracts.Repositories;
using Core.Contracts.Services;
using Core.Models.Constants;
using Infrastructure.DataContext;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Serilog;
using TbcTaskApi.Middlewares;
using TbcTaskApi.Services;

namespace TbcTaskApi.Configurations;

internal static class ApplicationConfigurations
    {
        public static void ConfigureApplication(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment env, WebApplicationBuilder builder)
        {
            AddServices(services, configuration);
            AddDatabase(services, configuration, env);
            AddLocalization(builder);
            AddLogger(builder);
        }
        
        public static void UserLocalization(WebApplication app)
        {
            var localizationOptions = app.Services.GetService<IOptions<RequestLocalizationOptions>>();
            app.UseRequestLocalization(localizationOptions.Value);
            app.UseMiddleware<LocalizationMiddleware>();
        }

        private static void AddServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IConnectionService, ConnectionService>();
            
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<IConnectionRepository, ConnectionRepository>();
            
            services.AddTransient<IUnitOfWork, UnitOfWork>();
        }

        private static void AddDatabase(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment env)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString(ConfigurationConstants.SqlConnection),
                    b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));
        }

        private static void AddLocalization(WebApplicationBuilder builder)
        {
            builder.Services.AddLocalization(options => options.ResourcesPath = "Shared");
            var supportedCultures = new[] { "en-US", "ka-GE"};
            builder.Services.Configure<RequestLocalizationOptions>(options =>
            {
                options.DefaultRequestCulture = new Microsoft.AspNetCore.Localization.RequestCulture("en-US");
                options.SupportedCultures = supportedCultures.Select(c => new CultureInfo(c)).ToList();
                options.SupportedUICultures = supportedCultures.Select(c => new CultureInfo(c)).ToList();
                
                options.RequestCultureProviders = new[] { new AcceptLanguageHeaderRequestCultureProvider() };
            });
        }
        
        private static void AddLogger(WebApplicationBuilder builder)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Error()
                .WriteTo.File("Logs/error-log.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            builder.Logging.ClearProviders();
            builder.Host.UseSerilog();
        }
    }