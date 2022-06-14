using System;
using System.Threading;
using System.Threading.Tasks;
using Notification.Domain.Models;
using Notification.Domain.Services;
using EventBus.Abstractions;
using EventBus.Events;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Notification.Host.Models.SignalR;

namespace Notification.Host.Models.Events
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
    }

    /// <inheritdoc />
    public class BillingEventHandler : IIntegrationEventHandler<BillingEvent>
    {
        private readonly INotificationEventService _notificationEventService;
        private readonly ILogger<BillingEventHandler> _logger;
        private readonly IHubContext<NotifyHub> _notifyHub;

        /// <summary>
        /// Constructor
        /// </summary>
        public BillingEventHandler(INotificationEventService notificationEventService, ILogger<BillingEventHandler> logger, IHubContext<NotifyHub> notifyHub)
        {
            _notificationEventService = notificationEventService;
            _logger = logger;
            _notifyHub = notifyHub;
        }

        /// <inheritdoc />
        public async Task Handle(BillingEvent @event)
        {
            
            _logger.LogInformation($"Get message {JsonConvert.SerializeObject(@event)}");
            await _notificationEventService.CreateNotificationEventAsync(@event.OperationId, @event.UserId, @event.Message, CancellationToken.None);
            await SendNotifyAsync(@event);
        }

        private async Task SendNotifyAsync(BillingEvent @event)
        {
            var connections = NotifyHub.GetConnections(@event.UserId);
            foreach (var connection in connections)
            {
                var client = _notifyHub.Clients.Client(connection);
                await NotifyHub.Notify(@event, client);
            }
        }
    }
}