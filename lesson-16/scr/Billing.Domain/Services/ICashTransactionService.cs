using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Billing.Domain.Models;

namespace Billing.Domain.Services
{
    /// <summary>
    /// Сервис управления записями кассовых операций
    /// </summary>
    public interface ICashTransactionService
    {
        /// <summary>
        /// Создание операции
        /// </summary>
        /// <returns></returns>
        Task<CashTransaction> CreateCashTransactionAsync(CashTransaction cashTransaction, CancellationToken cancellationToken);

        /// <summary>
        /// Получение операции
        /// </summary>
        Task<List<CashTransaction>> GetCashTransactionAsync(long userId, CancellationToken cancellationToken);
        
        /// <summary>
        /// Получение текущего состояния счета
        /// </summary>
        Task<decimal> GetTotalAmountAsync(long userId, CancellationToken cancellationToken);
    }
}