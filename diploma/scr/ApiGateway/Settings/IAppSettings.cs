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
        
        /// <summary>
        /// ServiceAccount using Pod to access the service of the k8s cluster
        /// needs to be ServiceAccount based on RBAC authorization
        /// </summary>
        bool UsePodServiceAccount { get; }
    }
}