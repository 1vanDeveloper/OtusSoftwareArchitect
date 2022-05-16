using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Billing.Domain.Models;

namespace Billing.Domain.Services
{
    /// <summary>
    /// Сервис управления записями сообщений
    /// </summary>
    public interface INotificationEventService
    {
        /// <summary>
        /// Создание сообщения
        /// </summary>
        Task CreateNotificationEventAsync(Guid operationId, long userId, string message, CancellationToken cancellationToken);

        /// <summary>
        /// Получение сообщений на отправку
        /// </summary>
        Task<List<NotificationEvent>> GetNewNotificationEventsAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Пометить сообщение как отправленое
        /// </summary>
        /// <param name="event"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task IsSentAsync(NotificationEvent @event, CancellationToken cancellationToken);
        
        /// <summary>
        /// Удаление сообщения
        /// </summary>
        Task RemoveNotificationEventAsync(Guid operationId, CancellationToken cancellationToken);
    }
}