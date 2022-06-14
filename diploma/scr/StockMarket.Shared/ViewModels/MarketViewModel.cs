using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;
using StockMarket.Shared.Models;
using StockMarket.Shared.Services;
using StockMarket.Shared.ViewModels.Interfaces;

namespace StockMarket.Shared.ViewModels;

public class MarketViewModel: IMarketViewModel
{
    private readonly HttpClient _httpClient;
    private readonly IAccessTokenService _accessTokenService;
    private readonly INotificationService _notificationService;
    private HubConnection _hubConnection;
    private readonly Dictionary<DateTime, FinancialData> _data = new ();
    
    public MarketViewModel(HttpClient httpClient, IAccessTokenService accessTokenService, INotificationService notificationService)
    {
        _httpClient = httpClient;
        _accessTokenService = accessTokenService;
        _notificationService = notificationService;
    }

    public async Task OnInitializedAsync(NavigationManager navigationManager, Func<List<FinancialData>, Task> action)
    {
        await _notificationService.SubscribeToNotificationAsync();
        
        _hubConnection = new HubConnectionBuilder()
            .WithUrl($"{_httpClient.BaseAddress}api/notification/stock", options =>
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
        
        _hubConnection.On<FinancialData[]>("TransferData", async array =>
        {
            foreach (var data in array)
            {
                _data[data.Time.Date] = data;
            }
            
            await action.Invoke(_data.Values.OrderBy(v => v.Time).ToList());
        });

        await _hubConnection.StartAsync();
    }

    public bool IsConnected =>
        _hubConnection?.State == HubConnectionState.Connected;

    public async ValueTask DisposeAsync()
    {
        if (_hubConnection is not null)
        {
            await _hubConnection.DisposeAsync();
        }
    }
}