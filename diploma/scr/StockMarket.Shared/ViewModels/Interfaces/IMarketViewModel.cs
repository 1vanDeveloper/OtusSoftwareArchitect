using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using StockMarket.Shared.Models;

namespace StockMarket.Shared.ViewModels.Interfaces;

public interface IMarketViewModel
{
    bool IsConnected { get; }
    Task OnInitializedAsync(NavigationManager navigationManager, Func<List<FinancialData>, Task> financialDataAction, 
        Func<List<Order>, Task> ordersAction);
    ValueTask DisposeAsync();
    Task MakeOrderAsync(decimal orderCost);
}