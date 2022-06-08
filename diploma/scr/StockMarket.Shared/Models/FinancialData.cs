using System;

namespace StockMarket.Shared.Models
{
    /// <summary>
    /// Модель данных финансового сообщения
    /// </summary>
    public class FinancialData
    {
        public string Date { get; set; }
        public DateTime Time { get; set; }
        public double Open { get; set; }
        public double High { get; set; }
        public double Low { get; set; }
        public double Close { get; set; }
        public double Volume { get; set; }
        public int Index { get; set; }
    }
}