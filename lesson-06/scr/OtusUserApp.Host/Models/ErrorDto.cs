namespace OtusUserApp.Host.Models
{
    /// <summary>
    /// Ошибка
    /// </summary>
    public class ErrorDto
    {
        /// <summary>
        /// Код
        /// </summary>
        public int Code { get; set; }
        
        /// <summary>
        /// Сообщение
        /// </summary>
        public string Message { get; set; }
    }
}