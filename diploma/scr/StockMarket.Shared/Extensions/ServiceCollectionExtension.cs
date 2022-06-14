using System;
using System.Net.Http;
using Blazored.LocalStorage;
using Blazored.Toast;
using IgniteUI.Blazor.Controls;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using StockMarket.Shared.Logging;
using StockMarket.Shared.Models;
using StockMarket.Shared.Services;
using StockMarket.Shared.ViewModels;
using StockMarket.Shared.ViewModels.Interfaces;

namespace StockMarket.Shared.Extensions;

/// <summary>
/// Сахар на поключение сервисов
/// </summary>
public static class ServiceCollectionExtension
{
    public static IServiceCollection AddStockMarketServices(this IServiceCollection services,
        ApplicationSettings applicationSettings)
    {
        // blazored services
        services.AddBlazoredToast();
        services.AddBlazoredLocalStorage();

        // authetication & authorization
        services.AddOptions();
        services.AddAuthorizationCore();
        services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
        services.AddScoped<IAccessTokenService, WebAppAccessTokenService>();
        services.AddScoped(typeof(IIgniteUIBlazor), typeof(IgniteUIBlazor));
        services.AddScoped<INotificationService, NotificationService>();        
        
        // configuring http clients
        services.AddScoped(_ => new HttpClient {BaseAddress = new Uri(applicationSettings.BaseAddress)});

        // transactional named http clients
        void ClientConfigurator(HttpClient client)
        {
            client.BaseAddress = new Uri(applicationSettings.BaseAddress);
        }

        services.AddHttpClient<IAccountViewModel, AccountViewModel>("AccountViewModelClient", (Action<HttpClient>) ClientConfigurator);
        services.AddHttpClient<IMarketViewModel, MarketViewModel>("MarketViewModelClient", (Action<HttpClient>) ClientConfigurator);

        // authentication http clients
        services.AddHttpClient<ILoginViewModel, LoginViewModel>("LoginViewModelClient", (Action<HttpClient>) ClientConfigurator);
        services.AddHttpClient<IRegisterViewModel, RegisterViewModel>("RegisterViewModelClient", (Action<HttpClient>) ClientConfigurator);

        // logging
        services.AddLogging(logging => logging.SetMinimumLevel(LogLevel.Error));
        services.AddSingleton<LogQueue>();
        services.AddSingleton<LogReader>();
        services.AddSingleton<LogWriter>();
        services.AddSingleton<ILoggerProvider, ApplicationLoggerProvider>();
        services.AddHttpClient("LoggerJob", c => c.BaseAddress = new Uri(applicationSettings.BaseAddress) );
        services.AddSingleton<LoggerJob>();

        return services;
    }
}