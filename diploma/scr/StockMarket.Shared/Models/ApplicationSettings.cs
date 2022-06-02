namespace StockMarket.Shared.Models;

/// <summary>
/// Настройки приложения
/// </summary>
public record ApplicationSettings
{
    /// <summary>
    /// Адрес серверной части
    /// </summary>
    public string BaseAddress { get; init; }
}