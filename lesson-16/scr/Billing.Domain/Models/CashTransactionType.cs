namespace Billing.Domain.Models
{
    /// <summary>
    /// Тип операции
    /// </summary>
    public enum CashTransactionType
    {
        /// <summary>
        /// Пополнение счета
        /// </summary>
        Debit = 0,
        
        /// <summary>
        /// Оплата
        /// </summary>
        Credit = 1000
    }
}