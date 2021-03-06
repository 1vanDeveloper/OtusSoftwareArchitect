namespace OtusUserApp.Host.Models
{
    /// <summary>
    /// Статус готовности
    /// </summary>
    public class HealthCheck
    {
        /// <summary>
        /// Статус
        /// </summary>
        public string Status { get; }

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="status"></param>
        public HealthCheck(string status)
        {
            Status = status;
        }
    }
}