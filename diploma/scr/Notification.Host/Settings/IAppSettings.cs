namespace Notification.Host.Settings
{
    /// <summary>
    /// Настройки приложения.
    /// </summary>
    public interface IAppSettings
    {
        /// <summary>
        /// Строка подключения к БД
        /// </summary>
        string DbConnectionString { get; }
        
        /// <summary>
        /// Строка подключения к Redis
        /// </summary>
        string RedisConnectionString { get; }

        /// <summary>
        /// Флаг указывает режим запуска сервиса
        /// </summary>
        bool IsMigrationService { get; }
        
        /// <summary>
        /// Адрес службы проверки прав пользователей
        /// </summary>
        string IdentityServerUrl { get; }
        
        /// <summary>
        /// Адрес службы информации пользователей
        /// </summary>
        string AccountServiceUrl { get; }
        
        /// <summary>
        /// Строка подключения к Rabbit MQ
        /// </summary>
        string EventBusConnection { get; }
        
        /// <summary>
        /// Rabbit MQ User
        /// </summary>
        string EventBusUserName { get; }
        
        /// <summary>
        /// Rabbit MQ Password
        /// </summary>
        string EventBusPassword { get; }
        
        /// <summary>
        /// Очередь для публикации
        /// </summary>
        string QueueName { get; }
    }
}