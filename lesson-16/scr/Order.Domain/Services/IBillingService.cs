using System;
using System.Threading;
using System.Threading.Tasks;

namespace Order.Domain.Services
{
    /// <summary>
    /// Сервис взаимодействия с биллингом
    /// </summary>
    public interface IBillingService
    {
        /// <summary>
        /// Списание средств при заказе
        /// </summary>
        Task<(bool result, string message)> BuyAsync(long userId, decimal amount, string description, Guid operationId,
            CancellationToken cancellationToken);
    }
}