using System.Threading;
using System.Threading.Tasks;
using Account.Domain.Models;

namespace Account.Domain.Services
{
    /// <summary>
    /// Сервис управления записями пользователей
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Создание пользователя
        /// </summary>
        Task<User> CreateUserAsync(User user, CancellationToken cancellationToken);

        /// <summary>
        /// Получение пользователя
        /// </summary>
        Task<User> GetUserAsync(string userName, CancellationToken cancellationToken);

        /// <summary>
        /// Обновление параметров пользователя
        /// </summary>
        Task UpdateUserAsync(User user, CancellationToken cancellationToken);

        /// <summary>
        /// Удаление пользователя
        /// </summary>
        Task RemoveUserAsync(string userName, CancellationToken cancellationToken);
    }
}