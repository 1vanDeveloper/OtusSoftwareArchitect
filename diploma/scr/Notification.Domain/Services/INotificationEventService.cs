using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Notification.Domain.Models;

namespace Notification.Domain.Services
{
    /// <summary>
    /// Сервис управления записями сообщений
    /// </summary>
    public interface INotificationEventService
    {
        /// <summary>
        /// Создание сообщения
        /// </summary>
        Task CreateNotificationEventAsync(Guid operationId, long userId, string message,
            CancellationToken cancellationToken);

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