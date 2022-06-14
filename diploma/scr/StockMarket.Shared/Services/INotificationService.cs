using System.Threading.Tasks;
using StockMarket.Shared.Models;

namespace StockMarket.Shared.Services;

public interface INotificationService
{
    public delegate Task BillingHandler(BillingEvent message);

    public Task SubscribeToNotificationAsync();
    
    public void SubscribeToNotification(BillingHandler billingEvent);
    
    public ValueTask StopAsync();
}