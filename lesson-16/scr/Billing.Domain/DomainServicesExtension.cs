using Billing.Domain.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Billing.Domain
{
    /// <summary>
    /// Класс инициализации сервисов домена
    /// </summary>
    public static class DomainServicesExtension
    {
        /// <summary>
        /// Инициализации сервисов домена
        /// </summary>
        /// <returns></returns>
        public static IServiceCollection AddBillingDomainServices(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connectionString));
            services.AddScoped<ICashTransactionService, CashTransactionService>();

            return services;
        }
    }
}