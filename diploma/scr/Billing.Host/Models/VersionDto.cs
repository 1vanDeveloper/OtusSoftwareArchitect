using System.Runtime.Serialization;

namespace Billing.Host.Models
{
    /// <summary>
    /// Описание версии приложения
    /// </summary>
    [DataContract]
    public record VersionDto
    {
        /// <summary>
        /// Версия
        /// </summary>
        [DataMember]
        public string Version { get; init; }
    }
}