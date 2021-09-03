using System.ComponentModel.DataAnnotations;
using Account.Domain.Models;

namespace Account.Host.Models
{
    /// <summary>
    /// Класс передачи изменяемых параметров о пользователе
    /// </summary>
    public record UserParamsDto
    {
        /// <summary>
        /// Constructor for deserializing
        /// </summary>
        public UserParamsDto()
        {
            
        }
        
        /// <summary>
        /// Constructor for <see cref="User"/> to UserParamsDto converting
        /// </summary>
        /// <param name="user"></param>
        public UserParamsDto(User user)
        {
            FirstName = user.FirstName;
            LastName = user.LastName;
            Email = user.Email;
            Phone = user.Phone;
        }
        
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

        /// <summary>
        /// Конвертация в <see cref="User"/>
        /// </summary>
        /// <returns></returns>
        public virtual User ConvertToUser()
        {
            return new User
            {
                FirstName = FirstName,
                LastName = LastName,
                Email = Email,
                Phone = Phone
            };
        }
    }
}