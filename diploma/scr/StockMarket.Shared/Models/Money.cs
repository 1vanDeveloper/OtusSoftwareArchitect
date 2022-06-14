namespace StockMarket.Shared.Models;

/// <summary>
/// Деньги
/// </summary>
public class Money
{
    /// <summary>
    /// Текущие срества
    /// </summary>
    public decimal Wallet { get; set; }
    
    /// <summary>
    /// Вносимые средства
    /// </summary>
    public decimal Deposit { get; set; }
}