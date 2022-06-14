using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Blazored.Toast.Services;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;
using StockMarket.Shared.ViewModels.Interfaces;
using StockMarket.Shared.Extensions;
using StockMarket.Shared.Models;
using StockMarket.Shared.Services;

namespace StockMarket.Shared.ViewModels;

///<inheritdoc />
public class LoginViewModel : ILoginViewModel
{
    private readonly HttpClient _httpClient;
    private readonly IToastService _toastService;
    private readonly IAccessTokenService _accessTokenService;

    [Required]
    [EmailAddress]
    public string EmailAddress { get; set; }
    
    [Required]
    public string Password { get; set; }
    
    public bool RememberMe { get; set; }

    public LoginViewModel(HttpClient httpClient, IToastService toastService, IAccessTokenService accessTokenService)
    {
        _httpClient = httpClient;
        _toastService = toastService;
        _accessTokenService = accessTokenService;
    }
    
    public async Task<string> AuthenticateJwtAsync()
    {
        //creating authentication request
        var content = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            { "client_id", "postman" },
            { "client_secret", "secret" },
            { "grant_type", "password" },
            { "username", EmailAddress },
            { "password", Password },
        });

        //authenticating the request
        var response = await _httpClient.PostAsync<AuthenticationResponse>("api/token", content);
        if (response.Code != HttpStatusCode.OK)
        {
            _toastService.ShowError(response.Error.Message);
            return null;
        }
        _toastService.ShowSuccess("Success!");
        
        await _accessTokenService.SetAccessTokenAsync("jwt_token", response.Response?.AccessToken);

        //sending the token to the client to store
        return response.Response?.AccessToken;
    }

    public User GetUser(string jwtToken)
    {
        if (string.IsNullOrEmpty(jwtToken))
        {
            return null;
        }
        
        var handler = new JwtSecurityTokenHandler();
        if (!handler.CanReadToken(jwtToken))
        {
            return null;
        }
        
        var token = handler.ReadToken(jwtToken) as JwtSecurityToken;
        var email = token?.Claims.First(claim => claim.Type == "email").Value;
        return new User
        {
            Email = email
        };
    }
}