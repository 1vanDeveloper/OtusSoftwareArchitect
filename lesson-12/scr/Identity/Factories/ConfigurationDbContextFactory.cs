using System.IO;
using System.Reflection;
using Identity.Data;
using Identity.Settings;
using IdentityServer4.EntityFramework.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Identity.Factories
{
    /// <inheritdoc />
    public class ConfigurationDbContextFactory : IDesignTimeDbContextFactory<AppConfigurationDbContext>
    {
        /// <summary>
        /// Create <see cref="AppConfigurationDbContext"/>
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public AppConfigurationDbContext CreateDbContext(string[] args)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory()))
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<AppConfigurationDbContext>();
            var storeOptions = new ConfigurationStoreOptions();

            var settings = new AppSettings(config);
            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;
            optionsBuilder.UseNpgsql(settings.ConnectionString, builder => builder.MigrationsAssembly(migrationsAssembly));

            return new AppConfigurationDbContext(optionsBuilder.Options, storeOptions);
        }
    }
}