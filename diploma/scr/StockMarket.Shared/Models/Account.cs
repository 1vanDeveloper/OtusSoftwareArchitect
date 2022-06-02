using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace StockMarket.Shared.Models;

/// <summary>
/// Полная информация о пользователе
/// </summary>
[DataContract]
public record Account
{
    /// <summary>
    /// Идентификатор
    /// </summary>
    [JsonPropertyName("id")]
    public long Id { get; init; }
    
    /// <summary>
    /// Имя
    /// </summary>
    [JsonPropertyName("firstName")]
    public string FirstName { get; set; }
        
    /// <summary>
    /// Фамилия
    /// </summary>
    [JsonPropertyName("lastName")] 
    public string LastName { get; set; }
        
    /// <summary>
    /// Почта
    /// </summary>
    [EmailAddress]
    [JsonPropertyName("email")]
    public string Email { get; init; }
        
    /// <summary>
    /// Телефон
    /// </summary>
    [Phone]
    [JsonPropertyName("phone")]
    public string Phone { get; set; }
}