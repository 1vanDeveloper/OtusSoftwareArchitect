using System.Threading.Tasks;
using OtusUserApp.Domain.Models;

namespace OtusUserApp.Domain.Services
{
    /// <inheritdoc cref="IUserService"/>
    public class UserService: IUserService
    {
        public Task<User> CreateUserAsync(User user)
        {
            throw new System.NotImplementedException();
        }

        public Task<User> GetUserAsync(long userId)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> TryUpdateUserAsync(User user)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> TryRemoveUserAsync(long userId)
        {
            throw new System.NotImplementedException();
        }
    }
}