using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Order.Host.Models
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
        public long Id { get; init; }

        /// <summary>
        /// Имя
        /// </summary>
        public string FirstName { get; init; }
        
        /// <summary>
        /// Фамилия
        /// </summary>
        public string LastName { get; init; }
        
        /// <summary>
        /// Почта
        /// </summary>
        [EmailAddress]
        public string Email { get; init; }
        
        /// <summary>
        /// Телефон
        /// </summary>
        [Phone]
        public string Phone { get; init; }
    }
}