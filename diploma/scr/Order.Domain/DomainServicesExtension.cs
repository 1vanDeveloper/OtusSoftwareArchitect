using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Order.Domain.Services;

namespace Order.Domain
{
    /// <summary>
    /// Класс инициализации сервисов домена
    /// </summary>
    public static class DomainServicesExtension
    {
        /// <summary>
        /// Инициализации сервисов домена
        /// </summary>
        public static IServiceCollection AddOrderDomainServices<TBillingService>(this IServiceCollection services, string connectionString) 
            where TBillingService : class, IBillingService
        {
            services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connectionString));
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IBillingService, TBillingService>();
            
            return services;
        }
    }
}