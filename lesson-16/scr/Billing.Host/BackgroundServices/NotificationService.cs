using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Billing.Domain.Services;
using Billing.Host.Models.Events;
using EventBus.Abstractions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Billing.Host.BackgroundServices
{
    /// <summary>
    /// Сервис отправки сообщений в очередь
    /// </summary>
    public class NotificationService : BackgroundService
    {
        private readonly ILogger<NotificationService> _logger;
        private readonly INotificationEventService _notificationEventService;
        private readonly IEventBus _eventBus;

        public NotificationService(ILogger<NotificationService> logger, 
            INotificationEventService notificationEventService, IEventBus eventBus)
        {
            _logger = logger;
            _notificationEventService = notificationEventService;
            _eventBus = eventBus;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogDebug("NotificationService is starting");

            stoppingToken.Register(() =>
                _logger.LogDebug("NotificationService background task is stopping"));

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogDebug("NotificationService task doing background work");

                var events = await _notificationEventService.GetNotificationEventAsync(stoppingToken);
                foreach (var @event in events.Select(e => new BillingEvent(e)))
                {
                    try
                    {
                        _eventBus.Publish(@event);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "ERROR Publishing integration event: {IntegrationEventId} from {AppName}", @event.Id, "Billing.Host");
                        throw;
                    }
                }

                await Task.Delay(TimeSpan.FromSeconds(1), stoppingToken);
            }

            _logger.LogDebug("NotificationService background task is stopping");
        }
    }
}