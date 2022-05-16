using System.Threading;
using System.Threading.Tasks;
using Notification.Host.Models;

namespace Notification.Host.Services
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