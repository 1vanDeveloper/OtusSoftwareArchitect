using System;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Billing.Domain.Models
{
    /// <summary>
    /// Касcовая операция
    /// </summary>
    [Index(nameof(OperationId), IsUnique = true)]
    public class CashTransaction
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        
        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        public long UserId { get; set; }
        
        /// <summary>
        /// Дата и время совершения операции
        /// </summary>
        public DateTime Timestamp { get; set; }
        
        /// <summary>
        /// Тип операции
        /// </summary>
        public CashTransactionType Type { get; set; }
        
        /// <summary>
        /// Сумма операции 
        /// </summary>
        public decimal Amount { get; set; }
        
        /// <summary>
        /// Описание операции
        /// </summary>
        public string Description { get; set; }
        
        /// <summary>
        /// Отменена
        /// </summary>
        public bool IsCanceled { get; set; }
        
        /// <summary>
        /// Уникльный идентификатор операции
        /// </summary>
        public Guid OperationId { get; set; }
    }
}