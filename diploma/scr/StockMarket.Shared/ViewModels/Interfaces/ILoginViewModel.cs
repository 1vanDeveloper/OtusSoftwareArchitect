using System;
using System.Threading.Tasks;
using StockMarket.Shared.Models;

namespace StockMarket.Shared.ViewModels.Interfaces;

/// <summary>
/// ViewModel входа пользователя
/// </summary>
public interface ILoginViewModel
{
    public string EmailAddress { get; set; }
    public string Password { get; set; }
    public bool RememberMe { get; set; }
    
    public Task<string> AuthenticateJwtAsync();
    public User GetUser(string jwtToken);
}