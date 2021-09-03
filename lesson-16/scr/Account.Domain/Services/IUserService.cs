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
        /// <param name="user"></param>
        /// <returns></returns>
        Task<User> CreateUserAsync(User user);

        /// <summary>
        /// Получение пользователя
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        Task<User> GetUserAsync(string userName);

        /// <summary>
        /// Обновление параметров пользователя
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task UpdateUserAsync(User user);

        /// <summary>
        /// Удаление пользователя
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        Task RemoveUserAsync(string userName);
    }
}