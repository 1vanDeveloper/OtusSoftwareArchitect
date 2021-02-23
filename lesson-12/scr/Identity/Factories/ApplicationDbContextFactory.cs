using System.IO;
using System.Reflection;
using Identity.Data;
using Identity.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Identity.Factories
{
    /// <inheritdoc />
    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        /// <summary>
        /// Create <see cref="ApplicationDbContext"/>
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory()))
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();

            var settings = new AppSettings(config);
            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;
            optionsBuilder.UseNpgsql(settings.ConnectionString, builder => builder.MigrationsAssembly(migrationsAssembly));

            return new ApplicationDbContext(optionsBuilder.Options);
        }
    }
}