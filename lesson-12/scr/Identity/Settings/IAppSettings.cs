namespace Identity.Settings
{
    /// <summary>
    /// Настройки приложения.
    /// </summary>
    internal interface IAppSettings
    {
        /// <summary>
        /// Identity server URL for Jwt Issuer Token 
        /// </summary>
        string IdentityServerUrl { get; }
        
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