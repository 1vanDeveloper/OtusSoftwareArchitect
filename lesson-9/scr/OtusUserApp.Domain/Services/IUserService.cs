using System.Threading.Tasks;
using OtusUserApp.Domain.Models;

namespace OtusUserApp.Domain.Services
{
    /// <summary>
    /// Сервис управления записями пользователей
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Создание пользователя
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<User> CreateUserAsync(User user);

        /// <summary>
        /// Получение пользователя
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<User> GetUserAsync(long userId);

        /// <summary>
        /// Обновление параметров пользователя
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task UpdateUserAsync(User user);

        /// <summary>
        /// Удаление пользователя
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task RemoveUserAsync(long userId);
    }
}