using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Notification.Host.Models
{
    /// <summary>
    /// Полная информация о пользователе
    /// </summary>
    [DataContract]
    public record UserDto
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        [DataMember]
        public long Id { get; init; }

        /// <summary>
        /// Имя
        /// </summary>
        [DataMember]
        public string FirstName { get; init; }
        
        /// <summary>
        /// Фамилия
        /// </summary>
        [DataMember]
        public string LastName { get; init; }
        
        /// <summary>
        /// Почта
        /// </summary>
        [DataMember]
        [EmailAddress]
        public string Email { get; init; }
        
        /// <summary>
        /// Телефон
        /// </summary>
        [DataMember]
        [Phone]
        public string Phone { get; init; }
    }
}