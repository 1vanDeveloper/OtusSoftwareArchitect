using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Account.Domain.Models
{
    /// <summary>
    /// Класс пользователя
    /// </summary>
    public class User
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        
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
        public string Email { get; set; }
        
        /// <summary>
        /// Телефон
        /// </summary>
        public string Phone { get; set; }
    }
}