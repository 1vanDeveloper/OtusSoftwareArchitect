using System.Runtime.Serialization;

namespace Notification.Host.Models
{
    /// <summary>
    /// Ошибка
    /// </summary>
    [DataContract]
    public record ErrorDto
    {
        /// <summary>
        /// Код
        /// </summary>
        [DataMember]
        public int Code { get; init; }
        
        /// <summary>
        /// Сообщение
        /// </summary>
        [DataMember]
        public string Message { get; init; }
    }
}