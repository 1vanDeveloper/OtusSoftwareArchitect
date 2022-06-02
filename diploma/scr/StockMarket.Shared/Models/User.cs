using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace StockMarket.Shared.Models;

/// <summary>
/// Полная информация о пользователе
/// </summary>
public record User
{
    /// <summary>
    /// Почта
    /// </summary>
    [EmailAddress]
    [JsonPropertyName("email")]
    public string Email { get; init; }
}