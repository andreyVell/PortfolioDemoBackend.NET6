using DataProvider;
using Microsoft.EntityFrameworkCore;

namespace WebApi.ApplicationConfiguration
{
    public static class DbContextExtensions
    {
        public static IServiceCollection AddCustomSqlContext(this IServiceCollection services, IConfiguration configuration)
        {
            //PostgreSQL connection
            string connectionString = string.Empty;
            var dbHost = Environment.GetEnvironmentVariable("DB_HOST");
            var dbPort = Environment.GetEnvironmentVariable("DB_PORT");
            var dbName = Environment.GetEnvironmentVariable("DB_NAME");
            var dbUsername = Environment.GetEnvironmentVariable("DB_USERNAME");
            var dbPassword = Environment.GetEnvironmentVariable("DB_PASSWORD");
            var dbAdditionalSettings = Environment.GetEnvironmentVariable("DB_ADDITIONAL_SETTINGS");
            if (string.IsNullOrWhiteSpace(dbHost))
                connectionString = configuration.GetConnectionString("PostgreeServerConnection")!;
            else
                connectionString = $"Host={dbHost};Port={dbPort};Database={dbName};Username={dbUsername};Password={dbPassword}; {dbAdditionalSettings}";
            services.AddDbContext<AvetonDbContext>(o =>
                o.UseNpgsql(connectionString)
                .UseProjectables()
                .UseLazyLoadingProxies()
                .LogTo(Console.WriteLine, LogLevel.Information));
            services.AddScoped<IAvetonDbContext>(provider => provider.GetService<AvetonDbContext>());
            var context = services.BuildServiceProvider().GetRequiredService<AvetonDbContext>();
            context?.Database.Migrate(); // DB automigration on start enable

            return services;
        }
    }
}
