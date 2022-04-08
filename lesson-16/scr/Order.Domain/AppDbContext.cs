using Microsoft.EntityFrameworkCore;
using Order.Domain.Models;

namespace Order.Domain
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
        /// Orders table
        /// </summary>
        public DbSet<Models.Order> Orders { get; set; }

        /// <inheritdoc />
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
            base.OnModelCreating(builder);
        }
    }
}