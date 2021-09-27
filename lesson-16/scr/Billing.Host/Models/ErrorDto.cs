namespace Billing.Host.Models
{
    /// <summary>
    /// Ошибка
    /// </summary>
    public record ErrorDto
    {
        /// <summary>
        /// Код
        /// </summary>
        public int Code { get; init; }
        
        /// <summary>
        /// Сообщение
        /// </summary>
        public string Message { get; init; }
    }
}