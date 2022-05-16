using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Options;
using Microsoft.EntityFrameworkCore;

namespace Identity.Data
{
    /// <inheritdoc />
    public class AppConfigurationDbContext: ConfigurationDbContext<AppConfigurationDbContext>
    {
        /// <inheritdoc />
        public AppConfigurationDbContext(DbContextOptions<AppConfigurationDbContext> options, ConfigurationStoreOptions storeOptions) 
            : base(options, storeOptions)
        {
        }
        
        /// <inheritdoc />
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder
                .UseSnakeCaseNamingConvention();

        /// <inheritdoc />
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(typeof(AppConfigurationDbContext).Assembly);
            builder.HasDefaultSchema("public");
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
            
            //Rename Identity tables to lowercase
            foreach (var entity in builder.Model.GetEntityTypes())
            {
                var currentTableName = builder.Entity(entity.Name).Metadata.GetDefaultTableName();
                builder.Entity(entity.Name).ToTable(currentTableName.ToLower());
            }
        }
    }
}