namespace OtusUserApp.Host
{
    /// <summary>
    /// Настройки приложения.
    /// </summary>
    public interface IAppSettings
    {
        /// <summary>
        /// Строка подключения к БД пользователей
        /// </summary>
        string UsersDbConnectionString { get; }

        /// <summary>
        /// Флаг указывает режим запуска сервиса
        /// </summary>
        bool IsMigrationService { get; }
    }
}