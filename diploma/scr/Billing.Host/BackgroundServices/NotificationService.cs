using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Billing.Domain.Services;
using Billing.Host.Models.Events;
using EventBus.Abstractions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

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

        /// <inheritdoc />
        public NotificationService(ILogger<NotificationService> logger, 
            INotificationEventService notificationEventService, IEventBus eventBus)
        {
            _logger = logger;
            _notificationEventService = notificationEventService;
            _eventBus = eventBus;
        }

        /// <inheritdoc />
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("NotificationService is starting");

            stoppingToken.Register(() =>
                _logger.LogInformation("NotificationService background task is stopping"));

            while (!stoppingToken.IsCancellationRequested)
            {
                var events = await _notificationEventService.GetNewNotificationEventsAsync(stoppingToken);
                foreach (var @event in events)
                {
                    try
                    {
                        _eventBus.Publish(BillingEvent.Convert(@event));
                        await _notificationEventService.IsSentAsync(@event, stoppingToken);
                        _logger.LogInformation($"NotificationService published {JsonConvert.SerializeObject(@event)}");
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "ERROR Publishing integration event: {IntegrationEventId} from {AppName}", @event.Id, "Billing.Host");
                        throw;
                    }
                }

                await Task.Delay(TimeSpan.FromSeconds(1), stoppingToken);
            }

            _logger.LogInformation("NotificationService background task is stopping");
        }
    }
}