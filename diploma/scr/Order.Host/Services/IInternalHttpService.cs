using System.Threading;
using System.Threading.Tasks;
using Order.Host.Models;

namespace Order.Host.Services
{
    /// <summary>
    /// Сервис внутренних http-соединений
    /// </summary>
    public interface IInternalHttpService
    {
        /// <summary>
        /// Получение информации о пользователе
        /// </summary>
        Task<UserDto> GetUserAsync(string userName, CancellationToken cancellationToken);
    }
}