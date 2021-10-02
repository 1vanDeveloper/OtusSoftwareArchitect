using System.Runtime.Serialization;
using Account.Domain.Models;

namespace Account.Host.Models
{
    /// <summary>
    /// Полная информация о пользователе
    /// </summary>
    [DataContract]
    public record UserDto: UserParamsDto
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
        [DataMember]
        public long Id { get; init; }

        /// <inheritdoc />
        public override User ConvertToUser()
        {
            var user = base.ConvertToUser();
            user.Id = Id;

            return user;
        }
    }
}