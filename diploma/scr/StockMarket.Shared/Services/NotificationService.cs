using System;
using System.Net.Http;
using System.Threading.Tasks;
using Blazored.Toast.Services;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;
using StockMarket.Shared.Models;

namespace StockMarket.Shared.Services;

public class NotificationService : INotificationService
{
    private readonly HttpClient _httpClient;
    private readonly IToastService _toastService;
    private readonly IAccessTokenService _accessTokenService;
    
    private static HubConnection _hubConnection;
    private event INotificationService.BillingHandler Notify;

    public NotificationService(HttpClient httpClient, IToastService toastService, IAccessTokenService accessTokenService)
    {
        _httpClient = httpClient;
        _toastService = toastService;
        _accessTokenService = accessTokenService;
    }

    public async Task SubscribeToNotificationAsync()
    {
        if (_hubConnection != null)
        {
            await StopAsync();
        }
        
        _hubConnection = new HubConnectionBuilder()
            .WithUrl($"{_httpClient.BaseAddress}api/notification/notify", options =>
            {
                options.AccessTokenProvider = async () => await _accessTokenService.GetAccessTokenAsync("jwt_token");
                options.Transports = HttpTransportType.LongPolling;
                options.HttpMessageHandlerFactory = innerHandler => 
                    new IncludeRequestCredentialsMessageHandler { InnerHandler = innerHandler };
            }).ConfigureLogging(builder =>
            {
                builder.SetMinimumLevel(LogLevel.Trace);
            })
            .Build();
        
        _hubConnection.On<BillingEvent>("Notify", async @event =>
        {
            _toastService.ShowInfo(@event.Message);
            if (Notify != null)
            {
                await Notify.Invoke(@event);
            }
        });

        await _hubConnection.StartAsync();
    }

    public void SubscribeToNotification(INotificationService.BillingHandler billingEvent)
    {
        Notify += billingEvent;
    }

    public async ValueTask StopAsync()
    {
        var allEvents = Notify?.GetInvocationList() ?? Array.Empty<Delegate>();
        foreach (var @event in allEvents)
            Notify -= @event as INotificationService.BillingHandler;
        
        if (_hubConnection is not null)
        {
            await _hubConnection.DisposeAsync();
            _hubConnection = null;
        }
    }
}