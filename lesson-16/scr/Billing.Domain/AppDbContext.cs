using Billing.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Billing.Domain
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
        /// CashTransaction table
        /// </summary>
        public DbSet<CashTransaction> CashTransactions { get; set; }
        
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