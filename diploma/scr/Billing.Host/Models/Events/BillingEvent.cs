using System;
using System.Threading;
using System.Threading.Tasks;
using Billing.Domain.Models;
using Billing.Domain.Services;
using EventBus.Abstractions;
using EventBus.Events;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Billing.Host.Models.Events
{
    /// <summary>
    /// Событие биллинга
    /// </summary>
    public class BillingEvent : IntegrationEvent
    {
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
        
        /// <summary>
        /// Convert
        /// </summary>
        public static BillingEvent Convert(NotificationEvent notificationEvent)
        {
            return new BillingEvent
            {
                OperationId = notificationEvent?.OperationId ?? default,
                UserId = notificationEvent?.UserId ?? default,
                Message = notificationEvent?.Message,
            };
        }
    }

    /// <inheritdoc />
    public class BillingEventHandler : IIntegrationEventHandler<BillingEvent>
    {
        private readonly INotificationEventService _notificationEventService;
        private readonly ILogger<BillingEventHandler> _logger;

        /// <summary>
        /// Constructor
        /// </summary>
        public BillingEventHandler(INotificationEventService notificationEventService, ILogger<BillingEventHandler> logger)
        {
            _notificationEventService = notificationEventService;
            _logger = logger;
        }

        /// <inheritdoc />
        public Task Handle(BillingEvent @event)
        {
            _logger.LogInformation($"Get message {JsonConvert.SerializeObject(@event)}");
            return _notificationEventService.RemoveNotificationEventAsync(@event.OperationId, CancellationToken.None);
        }
    }
}