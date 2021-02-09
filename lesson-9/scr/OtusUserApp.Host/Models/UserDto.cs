using OtusUserApp.Domain.Models;

namespace OtusUserApp.Host.Models
{
    /// <summary>
    /// Полная информация о пользователе
    /// </summary>
    public class UserDto: UserCreationDto
    {
        /// <inheritdoc />
        public UserDto()
        {
            
        }

        /// <inheritdoc />
        public UserDto(User user) : base(user)
        {
            Id = user.Id;
        }
        
        /// <summary>
        /// Идентификатор
        /// </summary>
        public long Id { get; set; }

        /// <inheritdoc />
        public override User ConvertToUser()
        {
            var user = base.ConvertToUser();
            user.Id = Id;

            return user;
        }
    }
}