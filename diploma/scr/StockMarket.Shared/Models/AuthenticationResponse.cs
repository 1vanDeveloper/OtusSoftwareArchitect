using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace StockMarket.Shared.Models;

/// <summary>
/// Ответ на запрос аутентификации пользователя
/// </summary>
public record AuthenticationResponse
{
    /// <summary>
    /// Тип токена
    /// </summary>
    [JsonPropertyName("token_type")]
    public string TokenType { get; init; }
    
    /// <summary>
    /// Тип доступа
    /// </summary>
    [JsonPropertyName("access_token")]
    public string AccessToken { get; init; }
};