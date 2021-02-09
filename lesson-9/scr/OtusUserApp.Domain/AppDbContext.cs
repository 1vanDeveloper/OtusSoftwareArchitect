using Microsoft.EntityFrameworkCore;
using OtusUserApp.Domain.Models;

namespace OtusUserApp.Domain
{
    /// <inheritdoc />
    public class AppDbContext: DbContext
    {
        /// <inheritdoc />
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        /// <summary>
        /// User table
        /// </summary>
        public DbSet<User> Users { get; set; }

        /// <inheritdoc />
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
            base.OnModelCreating(builder);
        }
    }
}