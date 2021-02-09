namespace OtusUserApp.Host.Models
{
    /// <summary>
    /// Описание версии приложения
    /// </summary>
    public class VersionDto
    {
        /// <summary>
        /// Версия
        /// </summary>
        public string Version { get; }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="version"></param>
        public VersionDto(string version)
        {
            Version = version;
        }
    }
}