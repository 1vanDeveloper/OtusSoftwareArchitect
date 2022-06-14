using System;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace StockMarket.Shared.Models;

/// <summary>
/// Результат пополнения счета
/// </summary>
public class MoneyResult
{
    /// <summary>
    /// Итого на счете 
    /// </summary>
    [JsonPropertyName("totalAmount")]
    public decimal TotalAmount { get; set; }
        
    /// <summary>
    /// Уникльный идентификатор операции
    /// </summary>
    [JsonPropertyName("operationId")]
    public Guid OperationId { get; set; }
}