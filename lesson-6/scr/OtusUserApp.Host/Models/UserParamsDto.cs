using System.ComponentModel.DataAnnotations;
using OtusUserApp.Domain.Models;

namespace OtusUserApp.Host.Models
{
    /// <summary>
    /// Класс передачи информации о пользователе по REST API
    /// </summary>
    public class UserParamsDto
    {
        public UserParamsDto()
        {
            
        }
        
        public UserParamsDto(User user)
        {
            UserName = user.UserName;
            FirstName = user.FirstName;
            LastName = user.LastName;
            Email = user.Email;
            Phone = user.Phone;
        }
        
        /// <summary>
        /// Логин
        /// </summary>
        [MaxLength(256)]
        public string UserName { get; set; }
        
        /// <summary>
        /// Имя
        /// </summary>
        public string FirstName { get; set; }
        
        /// <summary>
        /// Фамилия
        /// </summary>
        public string LastName { get; set; }
        
        /// <summary>
        /// Почта
        /// </summary>
        [EmailAddress]
        public string Email { get; set; }
        
        /// <summary>
        /// Телефон
        /// </summary>
        [Phone]
        public string Phone { get; set; }

        /// <summary>
        /// Конвертация в <see cref="User"/>
        /// </summary>
        /// <returns></returns>
        public virtual User ConvertToUser()
        {
            return new User
            {
                UserName = UserName,
                FirstName = FirstName,
                LastName = LastName,
                Email = Email,
                Phone = Phone
            };
        }
    }
}