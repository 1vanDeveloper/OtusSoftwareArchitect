namespace Order.Domain.Models
{
    /// <summary>
    /// Состояние заказа
    /// </summary>
    public enum OrderState
    {
        Unpaid = 0,
        
        Paid = 1,
        
        Canceled = 2
    }
} 