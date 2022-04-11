using Notification.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Notification.Domain
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
        /// NotificationEvent table
        /// </summary>
        public DbSet<NotificationEvent> NotificationEvents { get; set; }

        /// <inheritdoc />
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
            base.OnModelCreating(builder);
        }
    }
}