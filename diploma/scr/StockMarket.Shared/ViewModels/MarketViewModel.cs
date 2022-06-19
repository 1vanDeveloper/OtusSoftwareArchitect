using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;
using StockMarket.Shared.Extensions;
using StockMarket.Shared.Models;
using StockMarket.Shared.Services;
using StockMarket.Shared.ViewModels.Interfaces;

namespace StockMarket.Shared.ViewModels;

public class MarketViewModel: IMarketViewModel
{
    private readonly HttpClient _httpClient;
    private readonly IAccessTokenService _accessTokenService;
    private readonly INotificationService _notificationService;
    private readonly IToastService _toastService;
    private readonly Dictionary<DateTime, FinancialData> _data = new ();
    private Func<List<Order>, Task> _ordersAction;
    private HubConnection _hubConnection;
    private Guid _operationId = Guid.NewGuid();
    
    public MarketViewModel(HttpClient httpClient, IAccessTokenService accessTokenService, INotificationService notificationService,
        IToastService toastService)
    {
        _httpClient = httpClient;
        _accessTokenService = accessTokenService;
        _notificationService = notificationService;
        _toastService = toastService;
    }

    public async Task OnInitializedAsync(NavigationManager navigationManager, Func<List<FinancialData>, Task> financialDataAction, 
        Func<List<Order>, Task> ordersAction)
    {
        await _notificationService.SubscribeToNotificationAsync();
        await SubscribeToStockAsync(financialDataAction);
        _ordersAction = ordersAction;
        _notificationService.SubscribeToNotification(message => GetOrdersAsync());
        await GetOrdersAsync();
    }

    private async Task SubscribeToStockAsync(Func<List<FinancialData>, Task> action)
    {
        _hubConnection = new HubConnectionBuilder()
            .WithUrl($"{_httpClient.BaseAddress}api/notification/stock", options =>
            {
                options.AccessTokenProvider = async () => await _accessTokenService.GetAccessTokenAsync("jwt_token");
                options.Transports = HttpTransportType.LongPolling;
                options.HttpMessageHandlerFactory = innerHandler =>
                    new IncludeRequestCredentialsMessageHandler {InnerHandler = innerHandler};
            }).ConfigureLogging(builder => { builder.SetMinimumLevel(LogLevel.Trace); })
            .Build();

        _hubConnection.On<FinancialData[]>("TransferData", async array =>
        {
            foreach (var data in array)
            {
                _data[data.Time.Date] = data;
            }

            if (action != null)
            {
                await action.Invoke(_data.Values.OrderBy(v => v.Time).ToList());
            }
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
    
    private async Task GetOrdersAsync()
    {
        //preparing the http request
        var jwtToken = await _accessTokenService.GetAccessTokenAsync("jwt_token");
        const string url = "api/order/get-orders";

        //making the http request
        var response = await _httpClient.GetAsync<GetOrdersResult>(url, jwtToken);
        if (response.Code != HttpStatusCode.OK)
        {
            _toastService.ShowError(response.Error.Message);
        }
        else
        {
            if (_ordersAction != null)
            {
                await _ordersAction.Invoke(response.Response?.Orders?.OrderBy(v => v.Timestamp).ToList());
            }
        }
    }

    public async Task MakeOrderAsync(decimal orderCost)
    {
        var jwtToken = await _accessTokenService.GetAccessTokenAsync("jwt_token");
        const string url = "api/order/make-order";

        var response = await _httpClient.PostAsync(url, new MakeOrderParams
        {
            OperationId = _operationId,
            Amount = orderCost,
            Description = $"Microsoft order {DateTime.Now:yyyy-MM-dd HH:mm:ss}"
        }, jwtToken);

        _operationId = Guid.NewGuid();

        if (response.Code != HttpStatusCode.OK)
        {
            _toastService.ShowError(response.Error.Message);
        }
        else
        {
            _toastService.ShowSuccess("Success!");
            await GetOrdersAsync();
        }
    }
}