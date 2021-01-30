using System.Threading.Tasks;
using OtusUserApp.Domain.Models;

namespace OtusUserApp.Domain.Services
{
    /// <summary>
    /// Сервис управления записями пользователей
    /// </summary>
    public interface IUserService
    {
        Task<User> CreateUserAsync(User user);

        Task<User> GetUserAsync(long userId);

        Task UpdateUserAsync(User user);

        Task RemoveUserAsync(long userId);
    }
}