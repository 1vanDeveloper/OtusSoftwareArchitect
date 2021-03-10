namespace ApiGateway.Settings
{
    /// <summary>
    /// Настройки приложения.
    /// </summary>
    internal interface IAppSettings
    {
        /// <summary>
        /// Identity server URL
        /// </summary>
        string IdentityServerUrl { get; }
        
        /// <summary>
        /// Флаг запуска в оркестраторе
        /// </summary>
        bool IsInKubernetes { get; }
    }
}