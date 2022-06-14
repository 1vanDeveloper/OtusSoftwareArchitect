using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace StockMarket.Shared.Models;

/// <summary>
/// Ошибка
/// </summary>
public record Error
{
    /// <summary>
    /// Код
    /// </summary>
    [JsonPropertyName("code")]
    public int Code { get; init; }
        
    /// <summary>
    /// Сообщение
    /// </summary>
    [JsonPropertyName("message")]
    public string Message { get; init; }
}