using System.Threading;
using System.Threading.Tasks;
using Billing.Host.Models;

namespace Billing.Host.Services
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