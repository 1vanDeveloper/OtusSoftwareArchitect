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
        Task CreateNotificationEventAsync(Guid operationId, string message, CancellationToken cancellationToken);

        /// <summary>
        /// Получение сообщений
        /// </summary>
        Task<List<NotificationEvent>> GetNotificationEventAsync(CancellationToken cancellationToken);
        
        /// <summary>
        /// Удаление сообщения
        /// </summary>
        Task RemoveNotificationEventAsync(Guid operationId, CancellationToken cancellationToken);
    }
}