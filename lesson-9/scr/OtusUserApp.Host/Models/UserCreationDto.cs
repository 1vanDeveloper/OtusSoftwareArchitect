using System.ComponentModel.DataAnnotations;
using OtusUserApp.Domain.Models;

namespace OtusUserApp.Host.Models
{
    /// <summary>
    /// Класс входных параметров для создания пользователя
    /// </summary>
    public class UserCreationDto: UserParamsDto
    {
        /// <inheritdoc />
        public UserCreationDto()
        {
            
        }

        /// <inheritdoc />
        public UserCreationDto(User user) : base(user)
        {
            UserName = user.UserName;
        }
        
        /// <summary>
        /// Логин
        /// </summary>
        [MaxLength(256)]
        public string UserName { get; set; }

        /// <inheritdoc />
        public override User ConvertToUser()
        {
            var user = base.ConvertToUser();
            user.UserName = UserName;

            return user;
        }
    }
}