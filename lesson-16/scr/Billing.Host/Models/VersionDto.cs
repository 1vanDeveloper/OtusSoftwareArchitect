namespace Billing.Host.Models
{
    /// <summary>
    /// Описание версии приложения
    /// </summary>
    public record VersionDto
    {
        /// <summary>
        /// Версия
        /// </summary>
        public string Version { get; init; }
    }
}