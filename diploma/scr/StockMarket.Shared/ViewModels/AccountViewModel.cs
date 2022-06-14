using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Blazored.Toast.Services;
using StockMarket.Shared.Extensions;
using StockMarket.Shared.Models;
using StockMarket.Shared.Services;
using StockMarket.Shared.ViewModels.Interfaces;

namespace StockMarket.Shared.ViewModels;

public class AccountViewModel : IAccountViewModel
{
    private readonly HttpClient _httpClient;
    private readonly IAccessTokenService _accessTokenService;
    private readonly IToastService _toastService;
    private readonly INotificationService _notificationService;
    private Guid _operationId = Guid.NewGuid();

    public Account Account { get; private set; }
    public Money Money { get; private set; }

    public AccountViewModel(HttpClient httpClient, IAccessTokenService accessTokenService, IToastService toastService,
        INotificationService notificationService)
    {
        _httpClient = httpClient;
        _accessTokenService = accessTokenService;
        _toastService = toastService;
        _notificationService = notificationService;
    }
    
    public async Task InitAsync()
    {
        var jwtToken = await _accessTokenService.GetAccessTokenAsync("jwt_token");
        var handler = new JwtSecurityTokenHandler();
        var token = handler.ReadToken(jwtToken) as JwtSecurityToken;
        var email = token?.Claims.First(claim => claim.Type == "email").Value;
        if (string.IsNullOrEmpty(email))
        {
            _toastService.ShowError("Bad token!");
            return;
        }

        await _notificationService.SubscribeToNotificationAsync();
        await GetAccountAsync(email, jwtToken);
        await GetMoneyAsync(jwtToken);
    }
    
    
    private async Task GetAccountAsync(string email, string jwtToken)
    {
        //preparing the http request
        var url = $"api/account/{email}";

        //making the http request
        var response = await _httpClient.GetAsync<Account>(url, jwtToken);
        if (response.Code == HttpStatusCode.OK)
        {
            //returning the user if found
            if (response.Response != null)
            {
                Account = response.Response;
                return;
            }
        }

        if (response.Code != HttpStatusCode.NotFound)
        {
            _toastService.ShowError(response.Error.Message);
            Account = new Account();
            return;
        }


        url = "api/account";
        var request = new Account
        {
            Email = email,
            Phone = "8-888-888-88-88",
            FirstName = "John",
            LastName = "Doe"
        };
        Account = (await _httpClient.PostAsync<Account>(url, request, jwtToken)).Response;
    }

    public async Task UpdateAccountAsync()
    {
        var jwtToken = await _accessTokenService.GetAccessTokenAsync("jwt_token");
        var url = $"api/account/{Account.Email}";

        var response = await _httpClient.PutAsync(url, Account, jwtToken);
        if (response.Code != HttpStatusCode.OK)
        {
            _toastService.ShowError(response.Error.Message);
        }
        else
        {
            _toastService.ShowSuccess("Profile info has been saved successfully.");
        }
    }

    private async Task GetMoneyAsync(string jwtToken)
    {
        //preparing the http request
        const string url = "api/billing/check-money";

        //making the http request
        var response = await _httpClient.GetAsync<MoneyResult>(url, jwtToken);
        if (response.Code != HttpStatusCode.OK)
        {
            _toastService.ShowError(response.Error.Message);
            Money = new Money();
        }
        else
        {
            Money = new Money
            {
                Wallet = response.Response.TotalAmount,
                Deposit = 1_000
            };
        }
    }

    public async Task PutMoneyAsync()
    {
        var jwtToken = await _accessTokenService.GetAccessTokenAsync("jwt_token");
        const string url = "api/billing/put-money";

        var response = await _httpClient.PostAsync<MoneyResult>(url, new PutMoney
        {
            OperationId = _operationId,
            Amount = Money.Deposit
        }, jwtToken);

        _operationId = Guid.NewGuid();

        if (response.Code != HttpStatusCode.OK)
        {
            _toastService.ShowError(response.Error.Message);
        }
        else
        {
            Money = new Money
            {
                Wallet = response.Response.TotalAmount,
                Deposit = 1_000
            };
            _toastService.ShowSuccess("Success!");
        }
    }
}