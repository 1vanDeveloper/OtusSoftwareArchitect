namespace OtusUserApp.Host
{
    /// <summary>
    /// Настройки приложения.
    /// </summary>
    public interface IAppSettings
    {
        string UsersDbConnectionString { get; }
    }
}