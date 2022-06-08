using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http.Connections.Client;
using Microsoft.AspNetCore.SignalR.Client;
using StockMarket.Shared.Models;
using StockMarket.Shared.Services;
using StockMarket.Shared.ViewModels.Interfaces;

namespace StockMarket.Shared.ViewModels;

public class MarketViewModel: IMarketViewModel
{
    private readonly HttpClient _httpClient;
    private readonly IAccessTokenService _accessTokenService;
    private HubConnection _hubConnection;
    private readonly Dictionary<DateTime, FinancialData> _data = new ();
    
    public MarketViewModel(HttpClient httpClient, IAccessTokenService accessTokenService)
    {
        _httpClient = httpClient;
        _accessTokenService = accessTokenService;
    }

    public async Task OnInitializedAsync(NavigationManager navigationManager, Action<List<FinancialData>> action)
    {
        _hubConnection = new HubConnectionBuilder()
            .WithUrl($"{_httpClient.BaseAddress}api/notification/stock", options =>
            {
                //options.AccessTokenProvider = async () => await _accessTokenService.GetAccessTokenAsync("jwt_token");
            })
            .Build();

        _hubConnection.On<string, FinancialData[]>("TransferData", (user, array) =>
        {
            foreach (var data in array)
            {
                _data[data.Time.Date] = data;
            }
            action?.Invoke(_data.Values.ToList());
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