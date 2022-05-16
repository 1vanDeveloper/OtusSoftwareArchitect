using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Notification.Domain.Models
{
    /// <summary>
    /// Кассовая операция
    /// </summary>
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
    }
}