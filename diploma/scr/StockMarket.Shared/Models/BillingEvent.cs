using System;
using System.Text.Json.Serialization;

namespace StockMarket.Shared.Models;

/// <summary>
/// Событие биллинга
/// </summary>
public class BillingEvent
{
    /// <summary>
    /// Уникльный идентификатор операции
    /// </summary>
    [JsonPropertyName("operationId")]
    public Guid OperationId { get; set; }
        
    /// <summary>
    /// Тело операции
    /// </summary>
    [JsonPropertyName("message")]
    public string Message { get; set; }
}