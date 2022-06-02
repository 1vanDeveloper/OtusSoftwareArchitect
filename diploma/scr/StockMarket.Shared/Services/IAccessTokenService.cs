using System.Threading.Tasks;

namespace StockMarket.Shared.Services;

/// <summary>
/// Сервис CRUD для работы с токенами
/// </summary>
public interface IAccessTokenService
{
    Task<string> GetAccessTokenAsync(string tokenName);
    
    Task SetAccessTokenAsync(string tokenName, string tokenValue);
    
    Task RemoveAccessTokenAsync(string tokenName);
}