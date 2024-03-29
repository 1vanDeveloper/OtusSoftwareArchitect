using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Billing.Domain.Models
{
    /// <summary>
    /// Кассовая операция
    /// </summary>
    [Index(nameof(OperationId), IsUnique = true)]
    public class NotificationEvent
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        
        /// <summary>
        /// Уникльный идентификатор операции
        /// </summary>
        public Guid OperationId { get; set; }
        
        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        public long UserId { get; set; }
        
        /// <summary>
        /// Тело операции
        /// </summary>
        public string Message { get; set; }
        
        /// <summary>
        /// Сообщение отправелено
        /// </summary>
        public bool IsSent { get; set; }
    }
}