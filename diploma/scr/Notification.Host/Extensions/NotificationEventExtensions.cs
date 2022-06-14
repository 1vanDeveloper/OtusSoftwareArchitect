using Notification.Domain.Models;
using Notification.Host.Models.Events;

namespace Notification.Host.Extensions;

/// <summary>
/// 
/// </summary>
public static class NotificationEventExtensions
{
    /// <summary>
    /// Convert
    /// </summary>
    public static BillingEvent Convert(this NotificationEvent notificationEvent)
    {
        return new BillingEvent
        {
            OperationId = notificationEvent?.OperationId ?? default,
            UserId = notificationEvent?.UserId ?? default,
            Message = notificationEvent?.Message,
        };
    }
}