using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using StockMarket.Shared.Extensions;
using StockMarket.Shared.Models;
using StockMarket.Shared.Services;
using StockMarket.Shared.ViewModels.Interfaces;

namespace StockMarket.Shared.ViewModels;

public class AccountViewModel : IAccountViewModel
{
    private readonly HttpClient _httpClient;
    private readonly IAccessTokenService _accessTokenService;
    
    public Account Account { get; private set; }

    public AccountViewModel(HttpClient httpClient, IAccessTokenService accessTokenService)
    {
        _httpClient = httpClient;
        _accessTokenService = accessTokenService;
    }
    
    public async Task GetAsync()
    {
        var jwtToken = await _accessTokenService.GetAccessTokenAsync("jwt_token");
        var handler = new JwtSecurityTokenHandler();
        var token = handler.ReadToken(jwtToken) as JwtSecurityToken;
        var email = token?.Claims.First(claim => claim.Type == "email").Value;
        if (string.IsNullOrEmpty(email))
        {
            throw new Exception("Bad token!");
        }
        
        //preparing the http request
        var url = $"api/account/{email}";

        //making the http request
        var response = await _httpClient.GetAsync<Account>(url, jwtToken);
            
        //returning the user if found
        Account = response.Response;
        if (response.Response != null)
        {
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

    public async Task UpdateAsync()
    {
        var jwtToken = await _accessTokenService.GetAccessTokenAsync("jwt_token");
        var url = $"api/account/{Account.Email}";

        var response = await _httpClient.PutAsync<Account>(url, Account, jwtToken);
        if (response.Code != HttpStatusCode.OK)
        {
            throw new Exception("Update failed!");
        }
    }
}