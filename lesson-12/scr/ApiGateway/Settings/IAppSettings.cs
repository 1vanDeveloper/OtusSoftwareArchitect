namespace Identity.Settings
{
    /// <summary>
    /// Настройки приложения.
    /// </summary>
    internal interface IAppSettings
    {
        /// <summary>
        /// Строка подключения к БД
        /// </summary>
        string ConnectionString { get; }

        /// <summary>
        /// Флаг указывает режим запуска сервиса
        /// </summary>
        bool IsMigrationService { get; }
        
        /// <summary>
        /// Флаг запуска в оркестраторе
        /// </summary>
        bool IsInKubernetes { get; }
    }
}