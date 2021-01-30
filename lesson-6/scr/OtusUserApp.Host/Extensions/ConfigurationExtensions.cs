using System;
using Microsoft.Extensions.Configuration;

namespace OtusUserApp.Host.Extensions
{
    /// <summary>
    /// Класс расширений для <see cref="Microsoft.Extensions.Configuration.IConfiguration"/>.
    /// </summary>
    public static class ConfigurationExtensions
    {
        /// <summary>
        /// Получает значение по ключу, или выкидывает исключение если значение отсутствует или пусто.
        /// </summary>
        /// <param name="configuration"> Конфигурация приложения. </param>
        /// <param name="key"> Наименование ключа, по которому необходимо получить значение. </param>
        /// <typeparam name="T"> Тип значения. </typeparam>
        /// <returns> Значение. </returns>
        /// <exception> Исключение, выбрасываемое если значение отсутствует или пусто. </exception>
        public static T GetValueOrThrow<T>(this IConfiguration configuration, string key)
        {
            var section = configuration.GetSection(key);

            if (section.Exists() && !string.IsNullOrWhiteSpace(section.Value))
            {
                return configuration.GetValue<T>(key);
            }

            throw new Exception(
                $"'{key}' should be defined in app configuration (environment or appsettings).");
        }
    }
}