using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Order.Domain.Services
{
    /// <summary>
    /// Сервис управления заказами
    /// </summary>
    public interface IOrderService
    {
        /// <summary>
        /// Сделать заказ
        /// </summary>
        Task<Models.Order> MakeOrderAsync(Models.Order order, CancellationToken cancellationToken);

        /// <summary>
        /// Получить все заказы
        /// </summary>
        Task<List<Models.Order>> GetOrdersAsync(long userId, CancellationToken cancellationToken);
    }
}