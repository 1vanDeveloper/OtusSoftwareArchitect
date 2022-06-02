using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using StockMarket.Shared.ViewModels.Interfaces;
using StockMarket.Shared.Extensions;
using StockMarket.Shared.Models;

namespace StockMarket.Shared.ViewModels;

///<inheritdoc />
public class LoginViewModel : ILoginViewModel
{
    private readonly HttpClient _httpClient;
    
    [Required]
    [EmailAddress]
    public string EmailAddress { get; set; }
    
    [Required]
    public string Password { get; set; }
    
    public bool RememberMe { get; set; }
    
    public LoginViewModel()
    {
    }
    
    public LoginViewModel(HttpClient httpClient)
    {
        _httpClient = httpClient;
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