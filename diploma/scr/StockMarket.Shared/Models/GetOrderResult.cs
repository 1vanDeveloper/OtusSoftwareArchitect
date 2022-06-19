using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace StockMarket.Shared.Models;

/// <summary>
/// Список заказов
/// </summary>
public class GetOrdersResult
{
    /// <summary>
    /// Идентификатор пользователя
    /// </summary>
    [JsonPropertyName("orders"), Required]
    public List<Order> Orders { get; set; }
}

/// <summary>
/// Заказ
/// </summary>
public class Order
{
    /// <summary>
    /// Идентификатор
    /// </summary>
    [JsonPropertyName("id"), Required]
    public long Id { get; set; }
        
    /// <summary>
    /// Дата и время совершения операции
    /// </summary>
    [JsonPropertyName("timestamp"), Required]
    public DateTime Timestamp { get; set; }
        
    /// <summary>
    /// Состояние заказа
    /// </summary>
    [JsonPropertyName("state"), Required]
    public OrderState State { get; set; }

    [JsonIgnore]
    public string StateDescription => State switch
    {
        OrderState.Unpaid => "unpaid",
        OrderState.Paid => "paid",
        OrderState.Canceled => "canceled",
        _ => throw new ArgumentOutOfRangeException()
    };
        
    /// <summary>
    /// Сумма заказа 
    /// </summary>
    [JsonPropertyName("amount"), Required]
    public decimal Amount { get; set; }
        
    /// <summary>
    /// Описание заказа
    /// </summary>
    [JsonPropertyName("description"), Required]
    public string Description { get; set; }
        
    /// <summary>
    /// Уникльный идентификатор операции
    /// </summary>
    [JsonPropertyName("operationId"), Required]
    public Guid OperationId { get; set; }
}

/// <summary>
/// Состояние заказа
/// </summary>
public enum OrderState
{
    Unpaid = 0,
        
    Paid = 1,
        
    Canceled = 2
}