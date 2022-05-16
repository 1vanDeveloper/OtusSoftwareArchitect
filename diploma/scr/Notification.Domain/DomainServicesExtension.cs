using Notification.Domain.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Notification.Domain
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
        public static IServiceCollection AddNotificationDomainServices(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connectionString));
            services.AddScoped<INotificationEventService, NotificationEventService>();
            
            return services;
        }
    }
}