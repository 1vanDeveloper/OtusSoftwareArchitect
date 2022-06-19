using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace StockMarket.Shared.Models;

/// <summary>
/// Параметры заказа
/// </summary>
public class MakeOrderParams
{
    /// <summary>
    /// Сумма операции 
    /// </summary>
    [JsonPropertyName("amount"), Required]
    public decimal Amount { get; set; }
        
    /// <summary>
    /// Описание операции
    /// </summary>
    [JsonPropertyName("description")]
    public string Description { get; set; }
        
    /// <summary>
    /// Уникльный идентификатор операции
    /// </summary>
    [JsonPropertyName("operationId"), Required]
    public Guid OperationId { get; set; }
}