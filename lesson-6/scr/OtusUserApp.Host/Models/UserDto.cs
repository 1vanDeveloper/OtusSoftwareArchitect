using OtusUserApp.Domain.Models;

namespace OtusUserApp.Host.Models
{
    /// <inheritdoc cref="UserParamsDto"/>
    public class UserDto: UserParamsDto
    {
        public UserDto()
        {
            
        }

        public UserDto(User user) : base(user)
        {
            Id = user.Id;
        }
        
        /// <summary>
        /// Идентификатор
        /// </summary>
        public long Id { get; set; }

        public override User ConvertToUser()
        {
            var user = base.ConvertToUser();
            user.Id = Id;

            return user;
        }
    }
}