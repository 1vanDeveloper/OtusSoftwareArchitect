using System;
using System.Threading;
using System.Threading.Tasks;
using Notification.Domain.Models;
using Notification.Domain.Services;
using EventBus.Abstractions;
using EventBus.Events;

namespace Notification.Host.Models.Events
{
    /// <summary>
    /// Событие биллинга
    /// </summary>
    public class BillingEvent : IntegrationEvent
    {
        /// <inheritdoc />
        public BillingEvent(NotificationEvent notificationEvent)
        {
            OperationId = notificationEvent.OperationId;
            UserId = notificationEvent.UserId;
            Message = notificationEvent.Message;
        }
        
        /// <summary>
        /// Уникльный идентификатор операции
        /// </summary>
        public Guid OperationId { get; set; }
        
        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        public long UserId { get; set; }
        
        /// <summary>
        /// Тело операции
        /// </summary>
        public string Message { get; set; }
    }

    /// <inheritdoc />
    public class BillingEventHandler : IIntegrationEventHandler<BillingEvent>
    {
        private readonly INotificationEventService _notificationEventService;

        /// <summary>
        /// Constructor
        /// </summary>
        public BillingEventHandler(INotificationEventService notificationEventService)
        {
            _notificationEventService = notificationEventService;
        }

        /// <inheritdoc />
        public Task Handle(BillingEvent @event)
        {
            return _notificationEventService.CreateNotificationEventAsync(@event.OperationId, @event.UserId, @event.Message, CancellationToken.None);
        }
    }
}