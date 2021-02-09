using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OtusUserApp.Domain.Services;

namespace OtusUserApp.Domain
{
    /// <summary>
    /// Класс инициализации сервисов домена
    /// </summary>
    public static class DomainServicesExtension
    {
        /// <summary>
        /// Инициализации сервисов домена
        /// </summary>
        /// <param name="services"></param>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public static IServiceCollection AddDomainServices(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connectionString));
            services.AddScoped<IUserService, UserService>();

            return services;
        }
    }
}